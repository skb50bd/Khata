using Data.Core;

using Domain;
using Domain.Reports;
using Domain.Utils;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using static System.Decimal;

namespace Data.Persistence.Reports;

public class InflowReportRepository 
    : IReportRepository<PeriodicalReport<Inflow>>
{
    private readonly IDateTimeProvider _dateTime;
    private readonly KhataContext _db;
    private readonly KhataSettings _settings;

    public InflowReportRepository(
        KhataContext db,
        IOptionsMonitor<KhataSettings> settings, 
        IDateTimeProvider dateTime)
    {
        _db       = db;
        _dateTime = dateTime;
        _settings = settings.CurrentValue;
    }

    private Task<Inflow?> GetInflow(DateOnly date) =>
        GetInflow(date.ToDateTime(TimeOnly.MinValue));
    
    private async Task<Inflow?> GetInflow(DateTime fromDate)
    {
        var salesTask =
            _db.Set<Sale>()
                .Where(s => 
                    s.Metadata.CreationTime >= fromDate
                    && s.IsRemoved == false
                )
                .ToListAsync();

        var debtPaymentsTask =
            _db.Set<DebtPayment>()
                .Where(d => 
                    d.Metadata.CreationTime >= fromDate
                    && d.IsRemoved == false
                )
                .Select(d => new { d.Id, d.Amount })
                .ToListAsync();

        var purchaseReturnsTask =
            _db.Set<PurchaseReturn>()
                .Where(pr => 
                    pr.Metadata.CreationTime >= fromDate
                    && pr.IsRemoved == false
                )
                .Select(pr => new { pr.Id, pr.CashBack })
                .ToListAsync();

        var depositsTask =
            _db.Set<Deposit>()
                .Where(d =>
                    d.Metadata.CreationTime >= fromDate
                    && d.TableName == nameof(Deposit)
                )
                .Select(d => new { d.Id, d.Amount })
                .ToListAsync();

        await Task.WhenAll(
            salesTask, 
            debtPaymentsTask, 
            purchaseReturnsTask, 
            depositsTask
        );

        var sales           = await salesTask;
        var debtPayments    = await debtPaymentsTask;
        var purchaseReturns = await purchaseReturnsTask;
        var deposits        = await depositsTask;
        
        return new Inflow
        {
            DebtPaymentCount        = debtPayments.Count,
            DebtReceived            = Round(debtPayments.Sum(d => d.Amount), 2),
            DepositsCount           = deposits.Count,
            DepositAmount           = Round(deposits.Sum(d => d.Amount), 2),
            PurchaseReturnsCount    = purchaseReturns.Count,
            PurchaseReturnsReceived = Round(purchaseReturns.Sum(pr => pr.CashBack), 2),
            SaleCount               = sales.Count,
            SaleReceived            = Round(sales.Sum(s => s.Payment.Paid), 2),
            SaleProfit              = Round(sales.Sum(s => s.Profit), 2)
        };
    }

    public async Task<PeriodicalReport<Inflow>?> Get()
    {
        if (_settings.DbProvider is DbProvider.SQLServer)
        {
            var inflows =
                await _db.Set<Inflow>().ToListAsync();
            
            return new PeriodicalReport<Inflow>
            {
                Daily   = inflows[0],
                Weekly  = inflows[1],
                Monthly = inflows[2]
            };
        }

        var today       = _dateTime.Today;
        var dailyTask   = GetInflow(today);
        var weeklyTask  = GetInflow(today.StartOfTheWeek(DayOfWeek.Saturday));
        var monthlyTask = GetInflow(today.StartOfTheMonth());

        await Task.WhenAll(dailyTask, weeklyTask, monthlyTask);

        return new PeriodicalReport<Inflow>
        {
            ReportDate = today,
            Daily      = await dailyTask,
            Weekly     = await weeklyTask,
            Monthly    = await monthlyTask
        };
    }
}
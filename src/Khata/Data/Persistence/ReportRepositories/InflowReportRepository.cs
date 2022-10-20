using System;
using System.Linq;
using System.Threading.Tasks;

using Brotal.Extensions;

using Data.Core;

using Domain;
using Domain.Reports;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using static System.Decimal;

namespace Data.Persistence.Reports;

public class InflowReportRepository 
    : IReportRepository<PeriodicalReport<Inflow>>
{
    private readonly KhataContext _db;
    private readonly KhataSettings _settings;

    public InflowReportRepository(
        KhataContext db,
        IOptionsMonitor<KhataSettings> settings)
    {
        _db       = db;
        _settings = settings.CurrentValue;
    }

    private async Task<Inflow> GetInflow(DateTime fromDate)
    {
        var sales =
            await _db.Sales.Include(s => s.Cart)
                .Include(s => s.Metadata)
                .Where(
                    s => s.Metadata.CreationTime >= fromDate
                         && !s.IsRemoved)
                .ToListAsync();

        var debtPayments =
            await _db.DebtPayments.Include(d => d.Metadata)
                .Where(
                    d => d.Metadata.CreationTime >= fromDate
                         && !d.IsRemoved)
                .ToListAsync();

        var purchaseReturns =
            await _db.PurchaseReturns.Include(p => p.Metadata)
                .Where(
                    pr => pr.Metadata.CreationTime >= fromDate
                          && !pr.IsRemoved)
                .ToListAsync();

        var deposits =
            await _db.Deposits.Include(d => d.Metadata)
                .Where(d =>
                    d.Metadata.CreationTime >= fromDate
                    && d.TableName == nameof(Domain.Deposit))
                .ToListAsync();


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

    public async Task<PeriodicalReport<Inflow>> Get()
    {
        if (_settings.DbProvider == DbProvider.SQLServer)
        {
            var inflows =
                await _db.Set<Inflow>()
                    .ToListAsync();
            return new PeriodicalReport<Inflow>
            {
                Daily   = inflows[0],
                Weekly  = inflows[1],
                Monthly = inflows[2]
            };
        }

        var today   = Clock.Today;
        var daily   = await GetInflow(today);
        var weekly  = await GetInflow(today.StartOfWeek(DayOfWeek.Saturday));
        var monthly = await GetInflow(today.FirstDayOfMonth());

        return new PeriodicalReport<Inflow>
        {
            ReportDate = today,
            Daily = daily,
            Weekly  = weekly,
            Monthly = monthly
        };
    }
            
}
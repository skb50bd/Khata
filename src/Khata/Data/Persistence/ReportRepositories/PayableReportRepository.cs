using Data.Core;

using Domain;
using Domain.Reports;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Throw;
using static System.Decimal;

namespace Data.Persistence.Reports;

public class PayableReportRepository 
    : IReportRepository<PeriodicalReport<Payable>>
{
    private readonly IDateTimeProvider _dateTime;
    private readonly KhataContext  _db;
    private readonly KhataSettings _settings;

    public PayableReportRepository(
        KhataContext db,
        IOptionsMonitor<KhataSettings> settings, 
        IDateTimeProvider dateTime)
    {
        _db       = db;
        _dateTime = dateTime;
        _settings = settings.CurrentValue;
    }

    private Task<Payable?> GetPayable(DateOnly date) =>
        GetPayable(date.ToDateTime(TimeOnly.MinValue));
    
    private async Task<Payable?> GetPayable(DateTime fromDate)
    {
        var purchasesQuery =
            _db.Set<Purchase>()
                .Include(e => e.Metadata)
                .Where(e =>
                    e.Payment.Due > 0M
                    && e.Metadata.CreationTime >= fromDate
                    && e.IsRemoved == false
                );

        var purchasesCountTask = purchasesQuery.CountAsync();
        var purchasesSumTask   = purchasesQuery.SumAsync(x => x.Payment.Due);
        
        var salaryIssuesQuery =
            _db.Set<SalaryIssue>()
                .Include(e => e.Metadata)
                .Where(e => 
                    e.Metadata.CreationTime >= fromDate 
                    && e.IsRemoved == false
                );

        var salaryIssuesCountTask = salaryIssuesQuery.CountAsync();
        var salaryIssuesSumTask   = salaryIssuesQuery.SumAsync(x => x.Amount);
        
        var debtPaymentsQuery =
            _db.Set<DebtPayment>()
                .Include(e => e.Metadata)
                .Where(e => 
                    e.DebtAfter < 0M
                    && e.Metadata.CreationTime >= fromDate 
                    && e.IsRemoved == false
                );

        var debtPaymentsCountTask = debtPaymentsQuery.CountAsync();
        var debtPaymentsSumTask   = debtPaymentsQuery.SumAsync(d => d.DebtBefore >= 0 ? -d.DebtAfter : d.Amount);

        await Task.WhenAll(
            purchasesCountTask,
            purchasesSumTask,
            salaryIssuesCountTask,
            salaryIssuesSumTask,
            debtPaymentsCountTask,
            debtPaymentsSumTask
        );
        
        return new Payable
        {
            PurchaseDueCount      = await purchasesCountTask,
            PurchaseDueAmount     = Round(await purchasesSumTask, 2),
            SalaryIssueCount      = await salaryIssuesCountTask,
            SalaryIssueAmount     = Round(await salaryIssuesSumTask, 2),
            DebtOverPaymentCount  = await debtPaymentsCountTask,
            DebtOverPaymentAmount = Round(await debtPaymentsSumTask, 2)
        };
    }

    public async Task<PeriodicalReport<Payable>> Get()
    {
        if (_settings.DbProvider == DbProvider.SQLServer)
        {
            var payables =
                await _db.Set<Payable>().ToListAsync();

            payables.ThrowIfNull();
            
            return new PeriodicalReport<Payable>
            {
                Daily   = payables[0],
                Weekly  = payables[1],
                Monthly = payables[2]
            };
        }

        var today       = _dateTime.Today;
        var dailyTask   = GetPayable(today);
        var weeklyTask  = GetPayable(today.StartOfTheWeek(DayOfWeek.Saturday));
        var monthlyTask = GetPayable(today.StartOfTheMonth());

        await Task.WhenAll(
            dailyTask,
            weeklyTask,
            monthlyTask
        );
        
        return new PeriodicalReport<Payable>
        {
            ReportDate = today,
            Daily      = await dailyTask,
            Weekly     = await weeklyTask,
            Monthly    = await monthlyTask
        };
    }

}
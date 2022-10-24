using Data.Core;

using Domain;
using Domain.Reports;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data.Persistence.Reports;

public class ReceivableReportRepository 
    : IReportRepository<PeriodicalReport<Receivable>>
{
    private readonly IDateTimeProvider _dateTime;
    private readonly KhataContext  _db;
    private readonly KhataSettings _settings;

    public ReceivableReportRepository(
        KhataContext db,
        IOptionsMonitor<KhataSettings> settings, 
        IDateTimeProvider dateTime)
    {
        _db       = db;
        _dateTime = dateTime;
        _settings = settings.CurrentValue;
    }

    private Task<Receivable> GetReceivable(DateOnly date) =>
        GetReceivable(date.ToDateTime(TimeOnly.MinValue));
    
    private async Task<Receivable> GetReceivable(DateTime fromDate)
    {
        var salesQuery = 
            _db.Set<Sale>()
                .Where(e => 
                    e.Payment.Due > 0M
                    && e.Metadata.CreationTime >= fromDate
                    && e.IsRemoved == false
                );

        var salesCountTask = salesQuery.CountAsync();
        var salesSumTask   = salesQuery.SumAsync(x => x.Payment.Due);
        
        var supplierPaymentsQuery =
            _db.Set<SupplierPayment>()
                .Where(e => 
                    e.PayableAfter < 0M
                    && e.Metadata.CreationTime >= fromDate
                    && e.IsRemoved == false
                );

        var supplierPaymentsCountTask = supplierPaymentsQuery.CountAsync();
        var supplierPaymentsSumTask = 
            supplierPaymentsQuery
                .SumAsync(sp => 
                    sp.PayableBefore >= 0 
                        ? -sp.PayableAfter 
                        : sp.Amount
                );
        
        var salaryPaymentsQuery =
            _db.Set<SalaryPayment>()
                .Where(e => 
                    e.BalanceAfter < 0M
                    && e.Metadata.CreationTime >= fromDate
                    && e.IsRemoved == false
                );

        var salaryPaymentsCountTask = salaryPaymentsQuery.CountAsync();
        var salaryPaymentsSumTask = salaryPaymentsQuery.SumAsync(sp => sp.BalanceBefore >= 0 ? sp.BalanceAfter : sp.Amount);

        return new Receivable
        {
            SalesDueCount             = await salesCountTask,
            SalesDueAmount            = decimal.Round(await salesSumTask, 2),
            SupplierOverPaymentCount  = await supplierPaymentsCountTask,
            SupplierOverPaymentAmount = decimal.Round(await supplierPaymentsSumTask, 2),
            SalaryOverPaymentCount    = await salaryPaymentsCountTask,
            SalaryOverPaymentAmount   = decimal.Round(await salaryPaymentsSumTask, 2)
        };
    }

    public async Task<PeriodicalReport<Receivable>> Get()
    {
        if (_settings.DbProvider == DbProvider.SQLServer)
        {
            var receivables =
                await _db.Set<Receivable>()
                    .ToListAsync();
            return new PeriodicalReport<Receivable>
            {
                Daily   = receivables[0],
                Weekly  = receivables[1],
                Monthly = receivables[2]
            };
        }

        var today   = _dateTime.Today;
        var daily   = await GetReceivable(today);
        var weekly  = await GetReceivable(today.StartOfTheWeek(DayOfWeek.Saturday));
        var monthly = await GetReceivable(today.StartOfTheMonth());

        return new PeriodicalReport<Receivable>
        {
            ReportDate = today,
            Daily      = daily,
            Weekly     = weekly,
            Monthly    = monthly
        };
    }
}
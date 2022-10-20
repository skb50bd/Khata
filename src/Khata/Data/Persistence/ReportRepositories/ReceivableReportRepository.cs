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

public class ReceivableReportRepository 
    : IReportRepository<PeriodicalReport<Receivable>>
{
    private readonly KhataContext  _db;
    private readonly KhataSettings _settings;

    public ReceivableReportRepository(
        KhataContext                   db,
        IOptionsMonitor<KhataSettings> settings)
    {
        _db       = db;
        _settings = settings.CurrentValue;
    }
    private async Task<Receivable> GetReceivable(DateTime fromDate)
    {
        var sales = 
            await _db.Sales.Include(e => e.Metadata)
                .Where(e => e.Payment.Due > 0M
                            && e.Metadata.CreationTime >= fromDate
                            && !e.IsRemoved).ToListAsync();

        var supplierPayments =
            await _db.SupplierPayments.Include(e => e.Metadata)
                .Where(e => e.PayableAfter < 0M
                            && e.Metadata.CreationTime >= fromDate
                            && !e.IsRemoved).ToListAsync();

        var salaryPayments =
            await _db.SalaryPayments.Include(e => e.Metadata)
                .Where(e => e.BalanceAfter < 0M
                            && e.Metadata.CreationTime >= fromDate
                            && !e.IsRemoved).ToListAsync();

        return new Receivable
        {
            SalesDueCount             = sales.Count,
            SalesDueAmount            = Round(sales.Sum(s => s.Payment.Due), 2),
            SupplierOverPaymentCount  = supplierPayments.Count,
            SupplierOverPaymentAmount = 
                Round(supplierPayments.Sum(
                        sp => sp.PayableBefore >= 0 ? -sp.PayableAfter : sp.Amount),
                    2),
            SalaryOverPaymentCount    = salaryPayments.Count,
            SalaryOverPaymentAmount   = 
                Round(salaryPayments.Sum(
                        sp => sp.BalanceBefore >= 0 ? sp.BalanceAfter : sp.Amount), 
                    2)
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

        var today   = Clock.Today;
        var daily   = await GetReceivable(today);
        var weekly  = await GetReceivable(today.StartOfWeek(DayOfWeek.Saturday));
        var monthly = await GetReceivable(today.FirstDayOfMonth());

        return new PeriodicalReport<Receivable>
        {
            ReportDate = today,
            Daily      = daily,
            Weekly     = weekly,
            Monthly    = monthly
        };
    }

}
using System;
using System.Linq;
using System.Threading.Tasks;

using Brotal.Extensions;

using Data.Core;

using Domain.Reports;

using Microsoft.EntityFrameworkCore;

using static System.Decimal;

namespace Data.Persistence.Reports
{
    public class PayableReportRepository 
        : IReportRepository<PeriodicalReport<Payable>>
    {
        protected readonly KhataContext Db;
        public PayableReportRepository(KhataContext db)
            => Db = db;

        private async Task<Payable> GetPayable(DateTime fromDate)
        {
            var purchases =
                await Db.Purchases.Include(e => e.Metadata)
                  .Where(e => e.Payment.Due > 0M
                      && e.Metadata.CreationTime >= fromDate
                      && !e.IsRemoved)
                  .ToListAsync();

            var salaryIssues =
                await Db.SalaryIssues.Include(e => e.Metadata)
                        .Where(e => e.Metadata.CreationTime >= fromDate 
                          && !e.IsRemoved)
                        .ToListAsync();

            var debtPayments =
                await Db.DebtPayments.Include(e => e.Metadata)
                        .Where(e => e.DebtAfter < 0M
                             && e.Metadata.CreationTime >= fromDate 
                             && !e.IsRemoved)
                        .ToListAsync();

            return new Payable
            {
                PurchaseDueCount      = purchases.Count,
                PurchaseDueAmount     = Round(purchases.Sum(p => p.Payment.Due), 2),
                SalaryIssueCount      = salaryIssues.Count,
                SalaryIssueAmount     = Round(salaryIssues.Sum(si => si.Amount), 2),
                DebtOverPaymentCount  = debtPayments.Count,
                DebtOverPaymentAmount = 
                    Round(debtPayments.Sum(
                        d => d.DebtBefore >= 0 ? -d.DebtAfter : d.Amount),
                    2)
            };
        }

        public async Task<PeriodicalReport<Payable>> Get()
        {
            var today   = DateTime.Today;
            var daily   = await GetPayable(today);
            var weekly  = await GetPayable(today.StartOfWeek(DayOfWeek.Saturday));
            var monthly = await GetPayable(today.FirstDayOfMonth());

            return new PeriodicalReport<Payable>
            {
                ReportDate = today,
                Daily      = daily,
                Weekly     = weekly,
                Monthly    = monthly
            };
        }

    }
}

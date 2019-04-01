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

namespace Data.Persistence.Reports
{
    public class PayableReportRepository 
        : IReportRepository<PeriodicalReport<Payable>>
    {
        private readonly KhataContext  _db;
        private readonly KhataSettings _settings;

        public PayableReportRepository(
            KhataContext                   db,
            IOptionsMonitor<KhataSettings> settings)
        {
            _db       = db;
            _settings = settings.CurrentValue;
        }

        private async Task<Payable> GetPayable(DateTime fromDate)
        {
            var purchases =
                await _db.Purchases.Include(e => e.Metadata)
                  .Where(e => e.Payment.Due > 0M
                      && e.Metadata.CreationTime >= fromDate
                      && !e.IsRemoved)
                  .ToListAsync();

            var salaryIssues =
                await _db.SalaryIssues.Include(e => e.Metadata)
                        .Where(e => e.Metadata.CreationTime >= fromDate 
                          && !e.IsRemoved)
                        .ToListAsync();

            var debtPayments =
                await _db.DebtPayments.Include(e => e.Metadata)
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
            if (_settings.DbProvider == DbProvider.SQLServer)
            {
                var payables =
                    await _db.Query<Payable>()
                             .ToListAsync();
                return new PeriodicalReport<Payable>
                {
                    Daily   = payables[0],
                    Weekly  = payables[1],
                    Monthly = payables[2]
                };
            }

            var today   = Clock.Today;
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

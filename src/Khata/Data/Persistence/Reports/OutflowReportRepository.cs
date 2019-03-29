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
    public class OutflowReportRepository : IReportRepository<PeriodicalReport<Outflow>>
    {
        protected readonly KhataContext Db;
        public OutflowReportRepository(KhataContext db)
            => Db = db;

        private async Task<Outflow> GetOutflow(DateTime fromDate)
        {
            var expenses =
                await Db.Expenses.Include(e => e.Metadata)
                              .Where(e => e.Metadata.CreationTime > fromDate
                                       && !e.IsRemoved)
                              .ToListAsync();

            var purchases =
                await Db.Purchases.Include(e => e.Metadata)
                               .Where(e => e.Metadata.CreationTime > fromDate
                                        && !e.IsRemoved)
                               .ToListAsync();

            var salaryPayments =
                await Db.SalaryPayments.Include(e => e.Metadata)
                                    .Where(e => e.Metadata.CreationTime > fromDate
                                             && !e.IsRemoved)
                                    .ToListAsync();

            var supplierPayments =
                await Db.SupplierPayments.Include(e => e.Metadata)
                                      .Where(e => e.Metadata.CreationTime > fromDate
                                               && !e.IsRemoved)
                                      .ToListAsync();

            var refunds =
                await Db.Refunds.Include(e => e.Metadata)
                             .Where(e => e.Metadata.CreationTime > fromDate
                                      && !e.IsRemoved)
                             .ToListAsync();

            var withdrawals =
                await Db.Withdrawals.Include(e => e.Metadata)
                                 .Where(e => e.Metadata.CreationTime > fromDate
                                          && e.TableName == nameof(Domain.Withdrawal))
                                 .ToListAsync();

            return new Outflow
            {
                EmployeePaymentCount  = salaryPayments.Count,
                EmployeePaymentAmount = Round(salaryPayments.Sum(sp => sp.Amount), 2),
                ExpenseCount          = expenses.Count,
                ExpenseAmount         = Round(expenses.Sum(e => e.Amount), 2),
                PurchaseCount         = purchases.Count,
                PurchasePaid          = Round(purchases.Sum(p => p.Payment.Paid), 2),
                SupplierPaymentCount  = supplierPayments.Count,
                SupplierPaymentAmount = Round(supplierPayments.Sum(sp => sp.Amount), 2),
                RefundCount           = refunds.Count,
                RefundAmount          = Round(refunds.Sum(r => r.CashBack), 2),
                WithdrawalCount       = withdrawals.Count,
                WithdrawalAmount      = Round(withdrawals.Sum(w => w.Amount), 2)
            };
        }

        public async Task<PeriodicalReport<Outflow>> Get()
        {
            var today   = DateTime.Today;
            var daily   = await GetOutflow(today);
            var weekly  = await GetOutflow(today.StartOfWeek(DayOfWeek.Saturday));
            var monthly = await GetOutflow(today.FirstDayOfMonth());

            return new PeriodicalReport<Outflow>
            {
                ReportDate = today,
                Daily      = daily,
                Weekly     = weekly,
                Monthly    = monthly
            };
        }

    }
}

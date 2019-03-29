using System;
using System.Linq;
using System.Threading.Tasks;

using Brotal.Extensions;

using Data.Core;

using Domain.Reports;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Reports
{
    public class InflowReportRepository : IReportRepository<PeriodicalReport<Inflow>>
    {
        protected readonly KhataContext Db;
        public InflowReportRepository(KhataContext db)
            => Db = db;

        private async Task<Inflow> GetInflow(DateTime fromDate)
        {
            var sales =
                await Db.Sales.Include(s => s.Cart)
                           .Include(s => s.Metadata)
                           .Where(
                                s => s.Metadata.CreationTime >= fromDate
                                  && !s.IsRemoved)
                           .ToListAsync();

            var debtPayments =
                await Db.DebtPayments.Include(d => d.Metadata)
                                  .Where(
                                       d => d.Metadata.CreationTime >= fromDate
                                         && !d.IsRemoved)
                                  .ToListAsync();

            var purchaseReturns =
                await Db.PurchaseReturns.Include(p => p.Metadata)
                                     .Where(
                                          pr => pr.Metadata.CreationTime >= fromDate
                                             && !pr.IsRemoved)
                                     .ToListAsync();

            var deposits =
                await Db.Deposits.Include(d => d.Metadata)
                              .Where(d =>
                                   d.Metadata.CreationTime >= fromDate
                                   && d.TableName == nameof(Domain.Deposit))
                              .ToListAsync();


            return new Inflow
            {
                DebtPaymentCount        = debtPayments.Count,
                DebtReceived            = Decimal.Round(debtPayments.Sum(d => d.Amount), 2),
                DepositsCount           = deposits.Count,
                DepositAmount           = Decimal.Round(deposits.Sum(d => d.Amount), 2),
                PurchaseReturnsCount    = purchaseReturns.Count,
                PurchaseReturnsReceived = Decimal.Round(purchaseReturns.Sum(pr => pr.CashBack), 2),
                SaleCount               = sales.Count,
                SaleReceived            = Decimal.Round(sales.Sum(s => s.Payment.Paid), 2),
                SaleProfit              = Decimal.Round(sales.Sum(s => s.Profit), 2)
            };
        }

        public async Task<PeriodicalReport<Inflow>> Get()
        {
            var today   = DateTime.Today;
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
}

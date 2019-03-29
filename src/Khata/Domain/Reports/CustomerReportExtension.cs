using System.Linq;

namespace Domain.Reports
{
    public static class CustomerReportExtension {
        public static CustomerReport GetReport(this Customer customer) =>
            customer == null ? new CustomerReport() : 
                new CustomerReport
                {
                    Id         = customer.Id,
                    FullName   = customer.FullName,
                    Phone      = customer.Phone,
                    Address    = customer.Address,
                    Debt       = customer.Debt,
                    SalesCount = customer.Purchases.Count(p => !p.IsRemoved),
                    SaleReceives = customer
                                  .Purchases.Where(p => !p.IsRemoved)
                                  .Sum(s => s.Payment.Paid),
                    SalesWorth = customer
                                .Purchases.Where(p => !p.IsRemoved)
                                .Sum(s => s.Payment.Total),
                    Profit = customer.Purchases.Where(p => !p.IsRemoved)
                                     .Sum(s => s.Profit),
                    DebtPaymentsCount =
                        customer.DebtPayments.Count(p => !p.IsRemoved),
                    DebtPaymentReceives = customer
                                         .DebtPayments.Where(p => !p.IsRemoved)
                                         .Sum(d => d.Amount),
                    RefundCount = customer.Refunds.Count(p => !p.IsRemoved),
                    RefundLoss = customer
                                .Refunds.Where(p => !p.IsRemoved)
                                .Sum(r => r.EffectiveLoss),
                    RefundAmount = customer
                                  .Refunds.Where(p => !p.IsRemoved)
                                  .Sum(r => r.TotalBackPaid)
                };
    }
}
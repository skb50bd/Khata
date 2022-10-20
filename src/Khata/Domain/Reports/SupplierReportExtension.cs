using System.Linq;

namespace Domain.Reports;

public static class SupplierReportExtension
{
    public static SupplierReport GetReport(this Supplier supplier) =>
        new SupplierReport
        {
            Id       = supplier.Id,
            FullName = supplier.FullName,
            Phone    = supplier.Phone,
            Address  = supplier.Address,
            Payable  = supplier.Payable,
            PurchasesCount = 
                supplier.Purchases.Count(p => !p.IsRemoved),
            PurchasePaid = 
                supplier.Purchases.Where(p => !p.IsRemoved)
                    .Sum(s => s.Payment.Paid),
            PurchasesWorth = 
                supplier.Purchases.Where(p => !p.IsRemoved)
                    .Sum(s => s.Payment.Total),
            SupplierPaymentsCount = 
                supplier.Payments.Count(p => !p.IsRemoved),
            SupplierPaymentPaid =
                supplier.Payments.Where(p => !p.IsRemoved).Sum(d => d.Amount),
            PurchaseReturnCount =
                supplier.PurchaseReturns.Count(p => !p.IsRemoved),
            PurchaseReturnAmount = 
                supplier.PurchaseReturns.Where(p => !p.IsRemoved)
                    .Sum(r => r.TotalBackPaid)
        };
}
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Queries;

namespace Data.Persistence
{
    public static class QueryBuilder
    {
        public static ModelBuilder BuildQueries(
            this ModelBuilder modelBuilder, 
            KhataContext db)
        {
            modelBuilder.Query<CustomerReport>().ToQuery(() =>
                db.Customers.Where(s => !s.IsRemoved)
                    .Select(c => new CustomerReport
                    {
                        Id                  = c.Id,
                        FullName            = c.FullName,
                        Phone               = c.Phone,
                        Address             = c.Address,
                        Debt                = c.Debt,
                        SalesCount          = c.Purchases.Count(p => !p.IsRemoved),
                        SaleReceives        = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Payment.Paid),
                        SalesWorth          = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Payment.Total),
                        Profit              = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Profit),
                        DebtPaymentsCount   = c.DebtPayments.Count(p => !p.IsRemoved),
                        DebtPaymentReceives = c.DebtPayments.Where(p => !p.IsRemoved).Sum(d => d.Amount),
                        RefundCount         = c.Refunds.Count(p => !p.IsRemoved),
                        RefundLoss          = c.Refunds.Where(p => !p.IsRemoved).Sum(r => r.EffectiveLoss),
                        RefundAmount        = c.Refunds.Where(p => !p.IsRemoved).Sum(r => r.TotalBackPaid)
                    })
            );

            modelBuilder.Query<SupplierReport>().ToQuery(() =>
                db.Suppliers.Where(s => !s.IsRemoved)
                    .Select(c => new SupplierReport
                    {
                        Id                    = c.Id,
                        FullName              = c.FullName,
                        Phone                 = c.Phone,
                        Address               = c.Address,
                        Payable               = c.Payable,
                        PurchasesCount        = c.Purchases.Count(p => !p.IsRemoved),
                        PurchasePaid          = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Payment.Paid),
                        PurchasesWorth        = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Payment.Total),
                        SupplierPaymentsCount = c.Payments.Count(p => !p.IsRemoved),
                        SupplierPaymentPaid   = c.Payments.Where(p => !p.IsRemoved).Sum(d => d.Amount),
                        PurchaseReturnCount   = c.PurchaseReturns.Count(p => !p.IsRemoved),
                        PurchaseReturnAmount  = c.PurchaseReturns.Where(p => !p.IsRemoved).Sum(r => r.TotalBackPaid)
                    })
            );

            modelBuilder.Query<AssetReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new AssetReport
                {
                    DueCount       = db.Customers.Count(c => !c.IsRemoved && c.Debt > 0),
                    TotalDue       = db.Customers.Where(c => !c.IsRemoved).Sum(c => c.Debt),
                    Cash           = db.CashRegister.FirstOrDefault().Balance,
                    InventoryCount = db.Products.Count(p => !p.IsRemoved && p.Inventory.TotalStock > 0),
                    InventoryWorth = db.Products.Where(p => !p.IsRemoved).Sum(p => p.Inventory.TotalStock * p.Price.Purchase)
                })
            );

            modelBuilder.Query<LiabilityReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new LiabilityReport
                {
                    DueCount        = db.Suppliers.Count(c => !c.IsRemoved && c.Payable > 0),
                    TotalDue        = db.Suppliers.Where(c => !c.IsRemoved).Sum(c => c.Payable),
                    UnpaidEmployees = db.Employees.Count(p => !p.IsRemoved && p.Balance > 0),
                    UnpaidAmount    = db.Employees.Where(p => !p.IsRemoved).Sum(p => p.Balance)
                })
            );

            modelBuilder.Query<DailyIncomeReport>()
                .ToView("DailyIncomeReport");

            modelBuilder.Query<WeeklyIncomeReport>()
                .ToView("WeeklyIncomeReport");

            modelBuilder.Query<MonthlyIncomeReport>()
                .ToView("MonthlyIncomeReport");


            modelBuilder.Query<DailyExpenseReport>()
                .ToView("DailyExpenseReport");

            modelBuilder.Query<WeeklyExpenseReport>()
                .ToView("WeeklyExpenseReport");

            modelBuilder.Query<MonthlyExpenseReport>()
                .ToView("MonthlyExpenseReport");


            modelBuilder.Query<DailyOutletSalesReport>()
                .ToView("DailyOutletSalesReport");

            modelBuilder.Query<WeeklyOutletSalesReport>()
                .ToView("WeeklyOutletSalesReport");

            modelBuilder.Query<MonthlyOutletSalesReport>()
                .ToView("MonthlyOutletSalesReport");


            modelBuilder.Query<PerDayReport>()
                .ToView("PerDayReport");

            return modelBuilder;
        }
    }
}

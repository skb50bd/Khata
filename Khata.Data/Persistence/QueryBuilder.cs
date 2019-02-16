using System;
using System.Linq;
using System.Linq.Expressions;

using Khata.Queries;

using Microsoft.EntityFrameworkCore;

namespace Khata.Data.Persistence
{
    public static class QueryBuilder
    {
        public static ModelBuilder BuildQueries(this ModelBuilder modelBuilder, KhataContext db)
        {
            modelBuilder.Query<CustomerReport>().ToQuery(() =>
                db.Customers.Where(s => !s.IsRemoved)
                    .Select(c => new CustomerReport
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        Phone = c.Phone,
                        Address = c.Address,
                        Debt = c.Debt,
                        SalesCount = c.Purchases.Where(p => !p.IsRemoved).Count(),
                        SaleReceives = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Payment.Paid),
                        SalesWorth = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Payment.Total),
                        Profit = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Profit),

                        DebtPaymentsCount = c.DebtPayments.Where(p => !p.IsRemoved).Count(),
                        DebtPaymentReceives = c.DebtPayments.Where(p => !p.IsRemoved).Sum(d => d.Amount),

                        RefundCount = c.Refunds.Where(p => !p.IsRemoved).Count(),
                        RefundLoss = c.Refunds.Where(p => !p.IsRemoved).Sum(r => r.EffectiveLoss),
                        RefundAmount = c.Refunds.Where(p => !p.IsRemoved).Sum(r => r.TotalBackPaid)
                    })
            );

            modelBuilder.Query<SupplierReport>().ToQuery(() =>
                db.Suppliers.Where(s => !s.IsRemoved)
                    .Select(c => new SupplierReport
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        Phone = c.Phone,
                        Address = c.Address,
                        Payable = c.Payable,
                        PurchasesCount = c.Purchases.Where(p => !p.IsRemoved).Count(),
                        PurchasePaid = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Payment.Paid),
                        PurchasesWorth = c.Purchases.Where(p => !p.IsRemoved).Sum(s => s.Payment.Total),

                        SupplierPaymentsCount = c.Payments.Where(p => !p.IsRemoved).Count(),
                        SupplierPaymentPaid = c.Payments.Where(p => !p.IsRemoved).Sum(d => d.Amount),

                        PurchaseReturnCount = c.PurchaseReturns.Where(p => !p.IsRemoved).Count(),
                        PurchaseReturnAmount = c.PurchaseReturns.Where(p => !p.IsRemoved).Sum(r => r.TotalBackPaid)
                    })
            );

            modelBuilder.Query<AssetReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new AssetReport
                {
                    DueCount = db.Customers.Count(c => !c.IsRemoved && c.Debt > 0),
                    TotalDue = db.Customers.Where(c => !c.IsRemoved).Sum(c => c.Debt),

                    Cash = db.CashRegister.FirstOrDefault().Balance,

                    InventoryCount = db.Products.Count(p => !p.IsRemoved && p.Inventory.TotalStock > 0),
                    InventoryWorth = db.Products.Where(p => !p.IsRemoved).Sum(p => p.Inventory.TotalStock * p.Price.Purchase)
                })
            );

            modelBuilder.Query<LiabilityReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new LiabilityReport
                {
                    DueCount = db.Suppliers.Count(c => !c.IsRemoved && c.Payable > 0),
                    TotalDue = db.Suppliers.Where(c => !c.IsRemoved).Sum(c => c.Payable),

                    UnpaidEmployees = db.Employees.Count(p => !p.IsRemoved && p.Balance > 0),
                    UnpaidAmount = db.Employees.Where(p => !p.IsRemoved).Sum(p => p.Balance)
                })
            );

            modelBuilder.Query<DailyIncomeReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new DailyIncomeReport
                {
                    SaleCount = db.Deposits.Where(
                        d => d.TableName == "Sale"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Count(),
                    SaleReceived = db.Deposits.Where(d => d.TableName == "Sale"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Sum(d => d.Amount),
                    DebtPaymentCount = db.Deposits.Where(d => d.TableName == "DebtPayment"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Count(),
                    DebtReceived = db.Deposits.Where(d => d.TableName == "DebtPayment"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Sum(d => d.Amount)
                })
            );
            modelBuilder.Query<WeeklyIncomeReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new WeeklyIncomeReport
                {
                    SaleCount = db.Deposits.Where(
                        d => d.TableName == "Sale"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Count(),
                    SaleReceived = db.Deposits.Where(d => d.TableName == "Sale"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Sum(d => d.Amount),
                    DebtPaymentCount = db.Deposits.Where(d => d.TableName == "DebtPayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Count(),
                    DebtReceived = db.Deposits.Where(d => d.TableName == "DebtPayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Sum(d => d.Amount)
                })
            );
            modelBuilder.Query<MonthlyIncomeReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new MonthlyIncomeReport
                {
                    SaleCount = db.Deposits.Where(
                        d => d.TableName == "Sale"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Count(),
                    SaleReceived = db.Deposits.Where(d => d.TableName == "Sale"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Sum(d => d.Amount),
                    DebtPaymentCount = db.Deposits.Where(d => d.TableName == "DebtPayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Count(),
                    DebtReceived = db.Deposits.Where(d => d.TableName == "DebtPayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Sum(d => d.Amount)
                })
            );

            modelBuilder.Query<DailyExpenseReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new DailyExpenseReport
                {
                    PurchaseCount = db.Withdrawals.Where(
                        d => d.TableName == "Purchase"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Count(),
                    PurchasePaid = db.Withdrawals.Where(d => d.TableName == "Purchase"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Sum(d => d.Amount),
                    SupplierPaymentCount = db.Withdrawals.Where(
                        d => d.TableName == "SupplierPayment"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Count(),
                    SupplierPaymentAmount = db.Withdrawals.Where(d => d.TableName == "SupplierPayment"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Sum(d => d.Amount),
                    EmployeePaymentCount = db.Withdrawals.Where(
                        d => d.TableName == "EmployeePayment"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Count(),
                    EmployeePaymentAmount = db.Withdrawals.Where(d => d.TableName == "EmployeePayment"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Sum(d => d.Amount),
                    WithdrawalCount = db.Withdrawals.Where(
                        d => d.TableName == "Withdrawal"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Count(),
                    WithdrawalAmount = db.Withdrawals.Where(d => d.TableName == "Withdrawal"
                            && d.Metadata.CreationTime >= DateTime.Today
                        ).Sum(d => d.Amount),
                })
            );

            modelBuilder.Query<WeeklyExpenseReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new WeeklyExpenseReport
                {
                    PurchaseCount = db.Withdrawals.Where(
                        d => d.TableName == "Purchase"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Count(),
                    PurchasePaid = db.Withdrawals.Where(d => d.TableName == "Purchase"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Sum(d => d.Amount),
                    SupplierPaymentCount = db.Withdrawals.Where(
                        d => d.TableName == "SupplierPayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Count(),
                    SupplierPaymentAmount = db.Withdrawals.Where(d => d.TableName == "SupplierPayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Sum(d => d.Amount),
                    EmployeePaymentCount = db.Withdrawals.Where(
                        d => d.TableName == "EmployeePayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Count(),
                    EmployeePaymentAmount = db.Withdrawals.Where(d => d.TableName == "EmployeePayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Sum(d => d.Amount),
                    WithdrawalCount = db.Withdrawals.Where(
                        d => d.TableName == "Withdrawal"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Count(),
                    WithdrawalAmount = db.Withdrawals.Where(d => d.TableName == "Withdrawal"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-7)
                        ).Sum(d => d.Amount),
                })
            );

            modelBuilder.Query<MonthlyExpenseReport>().ToQuery(() =>
                db.CashRegister.Select(cr => new MonthlyExpenseReport
                {
                    PurchaseCount = db.Withdrawals.Where(
                        d => d.TableName == "Purchase"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Count(),
                    PurchasePaid = db.Withdrawals.Where(d => d.TableName == "Purchase"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Sum(d => d.Amount),
                    SupplierPaymentCount = db.Withdrawals.Where(
                        d => d.TableName == "SupplierPayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Count(),
                    SupplierPaymentAmount = db.Withdrawals.Where(d => d.TableName == "SupplierPayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Sum(d => d.Amount),
                    EmployeePaymentCount = db.Withdrawals.Where(
                        d => d.TableName == "EmployeePayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Count(),
                    EmployeePaymentAmount = db.Withdrawals.Where(d => d.TableName == "EmployeePayment"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Sum(d => d.Amount),
                    WithdrawalCount = db.Withdrawals.Where(
                        d => d.TableName == "Withdrawal"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Count(),
                    WithdrawalAmount = db.Withdrawals.Where(d => d.TableName == "Withdrawal"
                            && d.Metadata.CreationTime >= DateTime.Today.AddDays(-30)
                        ).Sum(d => d.Amount),
                })
            );

            #region PerDayReportQuery
            Expression<Func<IQueryable<PerDayReport>>> perDayReport = () =>
                db.Deposits
                    .GroupBy(d => d.Metadata.CreationTime.Date)
                    .Select(g => new { Date = g.Key, CashIn = g.Sum(d => d.Amount) })
                .Join(
                    db.Withdrawals
                        .GroupBy(w => w.Metadata.CreationTime.Date)
                        .Select(g => new { Date = g.Key, CashOut = g.Sum(d => d.Amount) }),
                    d => d.Date,
                    w => w.Date,
                    (d, w) => new { d.Date, d.CashIn, w.CashOut }
                ).Join(
                    db.Sales
                        .GroupBy(s => s.Metadata.CreationTime.Date)
                        .Select(g => new { Date = g.Key, NewReceivable = g.Sum(s => s.Payment.Due) }),
                    o => o.Date,
                    n => n.Date,
                    (o, n) => new { o.Date, o.CashIn, o.CashOut, n.NewReceivable }
                ).Join(
                    db.Purchases
                        .GroupBy(p => p.Metadata.CreationTime.Date)
                        .Select(g => new { Date = g.Key, NewPayable = g.Sum(p => p.Payment.Due) }),
                    o => o.Date,
                    n => n.Date,
                    (o, n) => new { o.Date, o.CashIn, o.CashOut, o.NewReceivable, n.NewPayable }
                ).Join(
                    db.SalaryIssues
                        .GroupBy(s => s.Metadata.CreationTime.Date)
                        .Select(g => new { Date = g.Key, NewPayable = g.Sum(si => si.Amount) }),
                    o => o.Date,
                    n => n.Date,
                    (o, n) => new PerDayReport
                    {
                        Date = o.Date,
                        CashIn = o.CashIn,
                        CashOut = o.CashOut,
                        NewReceivable = o.NewReceivable,
                        NewPayable = o.NewPayable + n.NewPayable
                    }
                ).OrderByDescending(p => p.Date)
                .Take(100);
            #endregion
            //modelBuilder.Query<PerDayReport>().ToQuery(perDayReport);


            //#region PerDayReportView
            //db.Database.ExecuteSqlCommand(@"
            //    DROP VIEW IF EXISTS PerDayReport
            //");
            //db.Database.ExecuteSqlCommand(@"
            //    CREATE VIEW PerDayReport AS
            //        SELECT TOP(30) * FROM (
            //            SELECT  TOP(30) 
            //                    CAST(m.CreationTime AS DATE) as 'Date',
            //                    COALESCE(SUM(d.Amount), 0) AS CashIn, 
            //                    COALESCE(SUM(w.Amount), 0) AS CashOut, 
            //                    COALESCE(SUM(s.Payment_Subtotal - s.Payment_DiscountCash - s.Payment_Paid), 0) AS NewReceivable,
            //                    COALESCE(SUM(p.Payment_Subtotal - p.Payment_DiscountCash - p.Payment_Paid + COALESCE(si.Amount, 0)), 0) AS NewPayable
            //            FROM Metadata m
            //            FULL JOIN Deposits d ON d.MetadataId = m.Id
            //            FULL JOIN Withdrawals w ON w.MetadataId = m.Id
            //            FULL JOIN Sale s ON s.MetadataId = m.Id 
            //            FULL JOIN Purchases p ON p.MetadataId = m.Id
            //            FULL JOIN SalaryIssues si ON si.MetadataId = m.Id
            //            GROUP BY CAST(m.CreationTime AS DATE)
            //            ORDER BY 'Date' DESC
            //        ) SQ ORDER BY 'Date' ASC
            //");
            //#endregion

            modelBuilder.Query<PerDayReport>().ToView("PerDayReport");

            return modelBuilder;
        }
    }
}

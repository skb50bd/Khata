using System;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Khata.Data.Persistence
{
    public partial class KhataContext : IdentityDbContext
    {
        private readonly ILogger<KhataContext> _logger;
        public KhataContext(
            DbContextOptions<KhataContext> options,
            ILogger<KhataContext> logger)
                : base(options)
        {
            _logger = logger;
            System.Collections.Generic.IEnumerable<string> pm = Database.GetPendingMigrations();

            try
            {
                if (/*pm.Any()*/ true)
                {
                    Database.Migrate();

                    #region PerDayReportView
                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS PerDayReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW PerDayReport AS
                            SELECT TOP(30) * FROM (
                                SELECT  TOP(30) 
                                        CAST(m.CreationTime AS DATE) as 'Date',
                                        ROUND(COALESCE(SUM(d.Amount), 0), 2) AS CashIn, 
                                        ROUND(COALESCE(SUM(w.Amount), 0), 2) AS CashOut, 
                                        ROUND(COALESCE(SUM(s.Payment_Subtotal - s.Payment_DiscountCash - s.Payment_Paid), 0), 2) AS NewReceivable,
                                        ROUND(COALESCE(SUM(p.Payment_Subtotal - p.Payment_DiscountCash - p.Payment_Paid + COALESCE(si.Amount, 0)), 0), 2) AS NewPayable
                                FROM Metadata m
                                FULL JOIN Deposits d ON d.MetadataId = m.Id
                                FULL JOIN Withdrawals w ON w.MetadataId = m.Id
                                FULL JOIN Sale s ON s.MetadataId = m.Id AND s.IsRemoved = 'False'
                                FULL JOIN Purchases p ON p.MetadataId = m.Id AND s.IsRemoved = 'False'
                                FULL JOIN SalaryIssues si ON si.MetadataId = m.Id AND si.IsRemoved = 'False'
                                GROUP BY CAST(m.CreationTime AS DATE)
                                ORDER BY 'Date' DESC
                            ) SQ ORDER BY 'Date' ASC
                    ");
                    #endregion

                    #region Periodical Income Reports
                    Database.ExecuteSqlCommand(@"
                        CREATE OR ALTER FUNCTION dbo.incomeReport (
                                @fromDate DATETIMEOFFSET(7),
                                @toDate DATETIMEOFFSET(7))
                            RETURNS TABLE
                            AS
                            RETURN
                            (
                                SELECT CAST(@fromDate AS DATE)				AS FromDate,
                                        CAST(@toDate AS DATE)				AS ToDate,
                                        COUNT(s.Id)							AS SaleCount, 
                                        COALESCE(SUM(s.Payment_Paid), 0)	AS SaleReceived, 
                                        COUNT(dp.Id)						AS DebtPaymentCount, 
                                        COALESCE(SUM(dp.Amount), 0)			AS DebtReceived, 
                                        COUNT(pr.Id)						AS PurchaseReturnsCount, 
                                        COALESCE(SUM(pr.CashBack), 0)		AS PurchaseReturnsReceived, 
                                        COUNT(d.Id)							AS DepositsCount, 
                                        COALESCE(SUM(d.Amount), 0)			AS DepositAmount
                                    FROM Metadata m
                                    FULL JOIN Deposits d					ON d.MetadataId = m.Id AND d.TableName = 'Deposit'
                                    FULL JOIN PurchaseReturns pr			ON pr.MetadataId = m.Id AND pr.IsRemoved = 'False'
                                    FULL JOIN Sale s						ON s.MetadataId = m.Id  AND s.IsRemoved = 'False'
                                    FULL JOIN DebtPayments dp				ON dp.MetadataId = m.Id AND dp.IsRemoved = 'False'
                                    WHERE m.CreationTime BETWEEN @fromDate AND @toDate
                            )
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS DailyIncomeReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW DailyIncomeReport AS
                            SELECT * FROM dbo.incomeReport(CAST(GETDATE() AS DATE), GETDATE())
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS WeeklyIncomeReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW WeeklyIncomeReport AS
                            SELECT * FROM dbo.incomeReport(DATEADD(day, -7, GETDATE()), GETDATE())
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS MonthlyIncomeReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW MonthlyIncomeReport AS
                            SELECT * FROM dbo.incomeReport(DATEADD(day, -30, GETDATE()), GETDATE())
                    ");
                    #endregion

                    #region Periodical Expense Reports
                    Database.ExecuteSqlCommand(@"
                        CREATE OR ALTER FUNCTION dbo.expenseReport (
                                @fromDate DATETIMEOFFSET(7), 
                                @toDate DATETIMEOFFSET(7))
                            RETURNS TABLE
                            AS
                            RETURN
                            (
                                SELECT CAST(@fromDate AS DATE)			AS FromDate,
                                    CAST(@toDate AS DATE)				AS ToDate,
                                    COUNT(e.Id)							AS ExpenseCount, 
                                    COALESCE(SUM(e.Amount), 0)			AS ExpenseAmount,
                                    COUNT(p.Id)							AS PurchaseCount, 
                                    COALESCE(SUM(p.Payment_Paid), 0)	AS PurchasePaid, 
                                    COUNT(sp.Id)						AS SupplierPaymentCount, 
                                    COALESCE(SUM(sp.Amount), 0)			AS SupplierPaymentAmount, 
                                    COUNT(ep.Id)						AS EmployeePaymentCount, 
                                    COALESCE(SUM(ep.Amount), 0)			AS EmployeePaymentAmount, 
                                    COUNT(r.Id)							AS RefundCount, 
                                    COALESCE(SUM(r.CashBack), 0)		AS RefundAmount, 
                                    COUNT(w.Id)							AS WithdrawalCount, 
                                    COALESCE(SUM(w.Amount), 0)			AS WithdrawalAmount
                                FROM Metadata m
                                FULL JOIN Expenses e					ON e.MetadataId = m.Id AND e.IsRemoved = 'False'
                                FULL JOIN Purchases p					ON p.MetadataId = m.Id AND p.IsRemoved = 'False'
                                FULL JOIN SupplierPayments sp			ON sp.MetadataId = m.Id AND sp.IsRemoved = 'False'
                                FULL JOIN SalaryPayments ep				ON ep.MetadataId = m.Id AND ep.IsRemoved = 'False'
                                FULL JOIN Refunds r						ON r.MetadataId = m.Id AND r.IsRemoved = 'False'
                                FULL JOIN Withdrawals w					ON w.MetadataId = m.Id AND w.TableName = 'Withdrawal'
                                WHERE m.CreationTime BETWEEN @fromDate AND @toDate
                            )
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS DailyExpenseReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW DailyExpenseReport AS
                            SELECT * FROM dbo.expenseReport(CAST(GETDATE() AS DATE), GETDATE())
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS WeeklyExpenseReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW WeeklyExpenseReport AS
                            SELECT * FROM dbo.expenseReport(DATEADD(day, -7, GETDATE()), GETDATE())
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS MonthlyExpenseReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW MonthlyExpenseReport AS
                            SELECT * FROM dbo.expenseReport(DATEADD(day, -30, GETDATE()), GETDATE())
                    ");
                    #endregion

                    #region Periodical Payable Reports
                    Database.ExecuteSqlCommand(@"
                        CREATE OR ALTER FUNCTION dbo.payableReport (
                                @fromDate DATETIMEOFFSET(7), 
                                @toDate DATETIMEOFFSET(7))
                            RETURNS TABLE
                            AS
                            RETURN
                            (
                                SELECT 
	                                CAST(@fromDate AS DATE)				AS FromDate,
                                    CAST(@toDate AS DATE)				AS ToDate,
                                    COUNT(p.Id) AS PurchaseDueCount,
                                    ROUND(
		                                COALESCE(
			                                SUM(p.Payment_SubTotal 
					                                - p.Payment_DiscountCash 
					                                - p.Payment_Paid), 
		                                0), 
	                                2)	AS PurchaseDueAmount,
	                                COUNT(si.Id) AS SalaryIssueCount,
	                                ROUND(COALESCE(SUM(si.Amount), 0), 2) AS SalaryIssueAmount,
	                                COUNT(dp.Id) AS DebtOverPaymentCount,
	                                ROUND(
		                                COALESCE(
			                                SUM(dp.Amount - dp.DebtBefore), 
		                                0), 
	                                2)	AS DebtOverPaymentAmount
                                FROM Metadata m
                                FULL JOIN Purchases p ON p.MetadataId = m.Id  
	                                AND p.IsRemoved = 'False' 
	                                AND p.Payment_SubTotal - p.Payment_DiscountCash - p.Payment_Paid > 0
                                FULL JOIN SalaryIssues si ON si.MetadataId = m.Id 
	                                AND si.IsRemoved = 'False' 
                                FULL JOIN DebtPayments dp ON dp.MetadataId = m.Id 
	                                AND dp.IsRemoved = 'False' 
	                                AND dp.DebtBefore - dp.Amount < 0
                                WHERE m.CreationTime BETWEEN @fromDate AND @toDate
                            )
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS DailyPayableReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW DailyPayableReport AS
                            SELECT * FROM dbo.payableReport(CAST(GETDATE() AS DATE), GETDATE())
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS WeeklyPayableReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW WeeklyPayableReport AS
                            SELECT * FROM dbo.payableReport(DATEADD(day, -7, GETDATE()), GETDATE())
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS MonthlyPayableReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW MonthlyPayableReport AS
                            SELECT * FROM dbo.payableReport(DATEADD(day, -30, GETDATE()), GETDATE())
                    ");
                    #endregion

                    #region Periodical Receivable Reports
                    Database.ExecuteSqlCommand(@"
                        CREATE OR ALTER FUNCTION dbo.receivableReport (
                                @fromDate DATETIMEOFFSET(7), 
                                @toDate DATETIMEOFFSET(7))
                            RETURNS TABLE
                            AS
                            RETURN
                            (
                                SELECT 
	                                CAST(@fromDate AS DATE)				AS FromDate,
                                    CAST(@toDate AS DATE)				AS ToDate,
                                    COUNT(s.Id) AS SalesDueCount,
                                    ROUND(
		                                COALESCE(
			                                SUM(s.Payment_SubTotal 
					                                - s.Payment_DiscountCash 
					                                - s.Payment_Paid), 
		                                0), 
	                                2)	AS SalesDueAmount,
	                                COUNT(sp.Id) AS SupplierOverPaymentCount,
	                                ROUND(
		                                COALESCE(
			                                SUM(sp.Amount - sp.PayableBefore), 
		                                0), 
	                                2)	AS SupplierOverPaymentAmount,
	                                COUNT(ep.Id) AS SalaryOverPaymentCount,
	                                ROUND(
		                                COALESCE(
			                                SUM(ep.Amount - ep.BalanceBefore), 
		                                0), 
	                                2)	AS SalaryOverPaymentAmount
                                FROM Metadata m
                                FULL JOIN Sale s ON s.MetadataId = m.Id  
	                                AND s.IsRemoved = 'False' 
	                                AND s.Payment_SubTotal - s.Payment_DiscountCash - s.Payment_Paid > 0
                                FULL JOIN SupplierPayments sp ON sp.MetadataId = m.Id 
	                                AND sp.IsRemoved = 'False' 
	                                AND sp.PayableBefore - sp.Amount < 0
                                FULL JOIN SalaryPayments ep ON ep.MetadataId = m.Id 
	                                AND ep.IsRemoved = 'False' 
	                                AND ep.BalanceBefore - ep.Amount < 0
                                WHERE m.CreationTime BETWEEN @fromDate AND @toDate
                            )
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS DailyReceivableReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW DailyReceivableReport AS
                            SELECT * FROM dbo.receivableReport(CAST(GETDATE() AS DATE), GETDATE())
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS WeeklyReceivableReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW WeeklyReceivableReport AS
                            SELECT * FROM dbo.receivableReport(DATEADD(day, -7, GETDATE()), GETDATE())
                    ");

                    Database.ExecuteSqlCommand(@"
                        DROP VIEW IF EXISTS MonthlyReceivableReport
                    ");
                    Database.ExecuteSqlCommand(@"
                        CREATE VIEW MonthlyReceivableReport AS
                            SELECT * FROM dbo.receivableReport(DATEADD(day, -30, GETDATE()), GETDATE())
                    ");
                    #endregion
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not apply Migrations" + JsonConvert.SerializeObject(pm));
                _logger.LogError(e.Message);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.BuildEntities();
            modelBuilder.BuildQueries(this);
        }
    }
}
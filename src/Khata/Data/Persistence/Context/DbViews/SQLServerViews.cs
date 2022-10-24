using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

// ReSharper disable InconsistentNaming

namespace Data.Persistence.DbViews;

public static class SQLServerViews
{
    public static DatabaseFacade CreateAllSQLServerViews(
        this DatabaseFacade db) =>
        db.AssetReport()
            .LiabilityReport()
            .PerDayReport()
            .IncomeReport()
            .ExpenseReport()
            .PayableReport()
            .ReceivableReport();

    private static DatabaseFacade AssetReport(
        this DatabaseFacade db)
    {
        db.ExecuteSqlRawAsync(@"DROP VIEW IF EXISTS Asset").Wait();
        
        db.ExecuteSqlRawAsync(@"
                CREATE VIEW Asset AS
                    SELECT *
                    FROM
                        (SELECT TOP(1) Balance AS 'Cash' FROM CashRegister) cr,
                        (SELECT COUNT(Id) AS 'InventoryCount',
                            ROUND(COALESCE(SUM(
                                Price_Purchase * (Inventory_Stock + Inventory_Warehouse)), 0
                                 ), 2) AS 'InventoryWorth'
                         FROM Products WHERE IsRemoved = 'FALSE' AND Inventory_Stock + Inventory_Warehouse > 0) p,
                        (SELECT COUNT(Id) AS 'DueCount',
                                ROUND(COALESCE(SUM(Debt), 0), 2) AS 'TotalDue'
                         FROM Customers WHERE IsRemoved = 'FALSE' AND Debt > 0) c
            ")
            .Wait();

        return db;
    }

    private static DatabaseFacade LiabilityReport(
        this DatabaseFacade db)
    {
        db.ExecuteSqlRawAsync(@"
                DROP VIEW IF EXISTS Liability
            ").Wait();
        
        db.ExecuteSqlRawAsync(@"
                CREATE VIEW Liability AS
                    SELECT *
                    FROM (
                        SELECT COUNT(Id) AS 'UnpaidEmployees',
                               ROUND(COALESCE(SUM(Balance), 0), 2) AS 'UnpaidAmount'
                        FROM Employees WHERE IsRemoved = 'FALSE' AND Balance > 0
                    ) e,
                         (
                        SELECT COUNT(Id) AS 'DueCount',
                               ROUND(COALESCE(SUM(Payable), 0), 2) AS 'TotalDue'
                        FROM Suppliers WHERE IsRemoved = 'FALSE' AND Payable > 0
                    ) s
            ")
            .Wait();

        return db;
    }

    private static DatabaseFacade PerDayReport(
        this DatabaseFacade db)
    {
        db.ExecuteSqlRawAsync(@"
                DROP VIEW IF EXISTS PerDayReport
            ")
            .Wait();
        
        db.ExecuteSqlRawAsync(@"
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
                        FULL JOIN Sales s ON s.MetadataId = m.Id AND s.IsRemoved = 'False'
                        FULL JOIN Purchases p ON p.MetadataId = m.Id AND p.IsRemoved = 'False'
                        FULL JOIN SalaryIssues si ON si.MetadataId = m.Id AND si.IsRemoved = 'False'
                        GROUP BY CAST(m.CreationTime AS DATE)
                        ORDER BY 'Date' DESC
                    ) SQ ORDER BY 'Date' ASC
            ")
            .Wait();
        
        return db;
    }

    private static DatabaseFacade IncomeReport(
        this DatabaseFacade db)
    {
        db.ExecuteSqlRawAsync(@"
                CREATE OR ALTER FUNCTION dbo.inflowReport (
                        @fromDate DATETIMEOFFSET(7),
                        @toDate DATETIMEOFFSET(7))
                    RETURNS TABLE
                    AS
                    RETURN
                    (
                        SELECT CAST(@fromDate AS DATE)					AS FromDate,
                            CAST(@toDate AS DATE)						AS ToDate,
                            COUNT(sl.SaleId)							AS SaleCount, 
                            ROUND(COALESCE(SUM(sl.Payment_Paid), 0), 2)	AS SaleReceived, 
                            ROUND(COALESCE(SUM(sl.Profit), 0), 2)		AS SaleProfit, 
                            COUNT(dp.Id)								AS DebtPaymentCount, 
                            ROUND(COALESCE(SUM(dp.Amount), 0), 2)		AS DebtReceived, 
                            COUNT(pr.Id)								AS PurchaseReturnsCount, 
                            ROUND(COALESCE(SUM(pr.CashBack), 0), 2)		AS PurchaseReturnsReceived, 
                            COUNT(d.Id)									AS DepositsCount, 
                            ROUND(COALESCE(SUM(d.Amount), 0), 2)		AS DepositAmount
                        FROM Metadata m
                        FULL JOIN Deposits d			ON d.MetadataId = m.Id AND d.TableName = 'Deposit'
                        FULL JOIN PurchaseReturns pr	ON pr.MetadataId = m.Id AND pr.IsRemoved = 'False'
                        FULL JOIN DebtPayments dp		ON dp.MetadataId = m.Id AND dp.IsRemoved = 'False'
                        FULL JOIN 
                            (SELECT 
                                s.Id AS SaleId, s.Payment_Paid, 
                                COALESCE(SUM(sli.UnitPrice * sli.Quantity - sli.UnitPurchasePrice * sli.Quantity), 0) AS Profit,
                                s.MetadataId, s.IsRemoved
                            FROM Sales s
                            LEFT JOIN SaleLineItem sli ON sli.SaleId = s.Id AND s.IsRemoved = 'False'
                            GROUP BY s.Id, s.Payment_Paid, s.MetadataId, s.IsRemoved) sl 
                                                        ON sl.MetadataId = m.Id AND sl.IsRemoved = 'False'
                        WHERE m.CreationTime BETWEEN @fromDate AND @toDate
                    )
            ")
            .Wait();

        db.ExecuteSqlRawAsync(@"
                DROP VIEW IF EXISTS PeriodicalInflow
            ")
            .Wait();

        db.ExecuteSqlRawAsync(@"
                CREATE VIEW PeriodicalInflow AS
                    SELECT * FROM dbo.inflowReport(CAST(GETDATE() AS DATE), GETDATE())
                    UNION ALL
                    SELECT * FROM dbo.inflowReport(DATEADD(day, -7, CAST(GETDATE() AS DATE)), GETDATE())
                    UNION ALL
                    SELECT * FROM dbo.inflowReport(DATEADD(day, -30, CAST(GETDATE() AS DATE)), GETDATE())
            ")
            .Wait();

        return db;
    }

    private static DatabaseFacade ExpenseReport(
        this DatabaseFacade db)
    {
        db.ExecuteSqlRawAsync(@"
                CREATE OR ALTER FUNCTION dbo.outflowReport (
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
            ")
            .Wait();

        db.ExecuteSqlRawAsync(@"
                DROP VIEW IF EXISTS PeriodicalOutflow
            ")
            .Wait();
        
        db.ExecuteSqlRawAsync(@"
                CREATE VIEW PeriodicalOutflow AS
                    SELECT * FROM dbo.outflowReport(CAST(GETDATE() AS DATE), GETDATE())
                    UNION ALL            
                    SELECT * FROM dbo.outflowReport(DATEADD(day, -7, CAST(GETDATE() AS DATE)), GETDATE())
                    UNION ALL            
                    SELECT * FROM dbo.outflowReport(DATEADD(day, -30, CAST(GETDATE() AS DATE)), GETDATE())
            ")
            .Wait();

        return db;
    }

    private static DatabaseFacade PayableReport(
        this DatabaseFacade db)
    {
        db.ExecuteSqlRawAsync(@"
                CREATE OR ALTER FUNCTION dbo.payableReport (
                        @fromDate DATETIMEOFFSET(7), 
                        @toDate DATETIMEOFFSET(7))
                    RETURNS TABLE
                    AS
                    RETURN
                    (
                        SELECT 
                            CAST(@fromDate AS DATE) AS FromDate,
                            CAST(@toDate AS DATE)   AS ToDate,
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
                                    SUM(
                                        CASE
                                           WHEN (dp.DebtBefore > 0) THEN dp.Amount - dp.DebtBefore
                                           ELSE dp.Amount
                                        END
                                    ), 
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
            ")
            .Wait();

        db.ExecuteSqlRawAsync(@"
                DROP VIEW IF EXISTS PeriodicalPayableReport
            ")
            .Wait();

        db.ExecuteSqlRawAsync(@"
                CREATE VIEW PeriodicalPayableReport AS
                    SELECT * FROM dbo.payableReport(CAST(GETDATE() AS DATE), GETDATE())
                    UNION ALL                    
                    SELECT * FROM dbo.payableReport(DATEADD(day, -7, CAST(GETDATE() AS DATE)), GETDATE())
                    UNION ALL
                    SELECT * FROM dbo.payableReport(DATEADD(day, -30, CAST(GETDATE() AS DATE)), GETDATE())
            ")
            .Wait();

        return db;
    }

    private static DatabaseFacade ReceivableReport(
        this DatabaseFacade db)
    {
        db.ExecuteSqlRawAsync(@"
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
                                    SUM(
                                        CASE
                                           WHEN (sp.PayableBefore > 0) THEN sp.Amount - sp.PayableBefore
                                           ELSE sp.Amount
                                        END
                                    ), 
                                0), 
                            2)	AS SupplierOverPaymentAmount,
                            COUNT(ep.Id) AS SalaryOverPaymentCount,
                            ROUND(
                                COALESCE(
                                    SUM(
                                        CASE
                                           WHEN (ep.BalanceBefore > 0) THEN ep.Amount - ep.BalanceBefore
                                           ELSE ep.Amount
                                        END
                                    ), 
                                0), 
                            2)	AS SalaryOverPaymentAmount
                        FROM Metadata m
                        FULL JOIN Sales s ON s.MetadataId = m.Id  
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
            ")
            .Wait();

        db.ExecuteSqlRawAsync(@"
                DROP VIEW IF EXISTS PeriodicalReceivableReport
            ")
            .Wait();

        db.ExecuteSqlRawAsync(@"
                CREATE VIEW PeriodicalReceivableReport AS
                    SELECT * FROM dbo.receivableReport(CAST(GETDATE() AS DATE), GETDATE())
                    UNION ALL
                    SELECT * FROM dbo.receivableReport(DATEADD(day, -7,CAST(GETDATE() AS DATE)), GETDATE())
                    UNION ALL
                    SELECT * FROM dbo.receivableReport(DATEADD(day, -30, CAST(GETDATE() AS DATE)), GETDATE())
            ")
            .Wait();

        return db;
    }
}
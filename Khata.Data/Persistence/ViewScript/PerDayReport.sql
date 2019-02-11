CREATE VIEW PerDayReport AS
SELECT  TOP(100) 
        CAST(m.CreationTime AS DATE) as 'Date',
        COALESCE(SUM(d.Amount), 0) AS CashIn, 
        COALESCE(SUM(w.Amount), 0) AS CashOut, 
        COALESCE(SUM(s.Payment_Subtotal - s.Payment_DiscountCash - s.Payment_Paid), 0) AS NewReceivable,
        COALESCE(SUM(p.Payment_Subtotal - p.Payment_DiscountCash - p.Payment_Paid + COALESCE(si.Amount, 0)), 0) AS NewPayable
FROM Metadata m
FULL JOIN Deposits d ON d.MetadataId = m.Id
FULL JOIN Withdrawals w ON w.MetadataId = m.Id
FULL JOIN Sale s ON s.MetadataId = m.Id 
FULL JOIN Purchases p ON p.MetadataId = m.Id
FULL JOIN SalaryIssues si ON si.MetadataId = m.Id
GROUP BY CAST(m.CreationTime AS DATE)
ORDER BY 'Date' DESC
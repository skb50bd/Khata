USE Khata
SELECT  COUNT(c.Id) AS DueCount,
		SUM(c.Debt) AS TotalDue,
		SUM(cr.Balance) / COUNT(cr.Balance) as Cash,
		COUNT(p.Id) AS InventoryCount,
		SUM((p.Inventory_Stock + p.Inventory_Warehouse) * p.Price_Purchase) AS InventoryWorth
FROM Customers c
LEFT JOIN CashRegister cr ON cr.Id > 0
LEFT JOIN Products p ON p.Id > 0
WHERE c.IsRemoved = 'False' 
	AND c.Debt > 0 
	AND p.IsRemoved = 'False' 
	AND (p.Inventory_Stock + p.Inventory_Warehouse) > 0 

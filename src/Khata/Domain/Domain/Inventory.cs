using static Domain.StockStatus;

namespace Domain;

public class Inventory
{
    public decimal Stock { get; set; }

    public decimal Warehouse { get; set; }

    public decimal AlertAt { get; set; }

    public decimal TotalStock => Stock + Warehouse;

    public StockStatus Status
    {
        get
        {
            if (TotalStock >= 2 * AlertAt) return InStock;
            if (TotalStock > AlertAt)      return LimitedStock;
            if (TotalStock > 0)            return LowStock;
            if (TotalStock == 0)           return Empty;
            return Negative;
        }
    }

    public bool MoveToGodown(decimal quantity)
    {
        if (Stock < quantity) return false;

        Stock     -= quantity;
        Warehouse += quantity;
        return true;
    }

    public bool MoveToStock(decimal quantity)
    {
        if (Warehouse < quantity) return false;

        Warehouse -= quantity;
        Stock     += quantity;
        return true;
    }
}
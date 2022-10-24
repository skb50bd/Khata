using static Domain.StockStatus;

namespace Domain;

public class Inventory
{
    public decimal Stock { get; set; }

    public decimal Warehouse { get; set; }

    public decimal AlertAt { get; set; }

    public decimal TotalStock => Stock + Warehouse;

    public StockStatus Status => TotalStock switch
    {
        > 2 when TotalStock >= 2 * AlertAt => InStock,
        > 0 when TotalStock > AlertAt      => LimitedStock,
        > 0                                => LowStock,
        0                                  => Empty,
        _                                  => Negative
    };

    public bool MoveToGodown(decimal quantity)
    {
        if (Stock < quantity)
        {
            return false;
        }

        Stock     -= quantity;
        Warehouse += quantity;
        return true;
    }

    public bool MoveToStock(decimal quantity)
    {
        if (Warehouse < quantity)
        {
            return false;
        }

        Warehouse -= quantity;
        Stock     += quantity;
        return true;
    }
}
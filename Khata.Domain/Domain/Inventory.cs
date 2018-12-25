using static Khata.Domain.StockStatus;

namespace Khata.Domain
{
    public class Inventory
    {
        public decimal Stock { get; set; }

        public decimal Godown { get; set; }

        public decimal AlertAt { get; set; }

        public decimal TotalStock => Stock + Godown;

        public StockStatus Status
        {
            get
            {
                if (TotalStock >= 2 * AlertAt) return InStock;
                if (TotalStock > AlertAt) return LimitedStock;
                if (TotalStock > 0) return LowStock;
                if (TotalStock == 0) return Empty;
                return Negative;
            }
        }

        public bool MoveToGodown(decimal quantity)
        {
            if (Stock < quantity) return false;

            Stock -= quantity;
            Godown += quantity;
            return true;
        }

        public bool MoveToStock(decimal quantity)
        {
            if (Godown < quantity) return false;

            Godown -= quantity;
            Stock += quantity;
            return true;
        }
    }
}

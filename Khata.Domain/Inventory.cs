namespace Khata.Domain
{
    public class Inventory : TrackedEntity
    {
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public decimal InStock { get; set; }

        public decimal InGodown { get; set; }
    }
}

namespace Khata.Domain
{
    public class Product : TrackedEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Inventory Inventory { get; set; }

        public string Unit { get; set; }

        public Pricing Price { get; set; }

        public Metadata Metadata { get; set; }
    }
}
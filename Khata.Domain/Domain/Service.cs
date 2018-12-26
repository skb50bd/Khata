namespace Khata.Domain
{
    public class Service : TrackedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Metadata Metadata { get; set; }
    }
}
namespace Khata.Domain
{
    public class Service : TrackedDocument
    {
        public string Name { get; set; }

        public int OutletId { get; set; }
        public virtual Outlet Outlet { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
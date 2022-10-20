namespace Domain;

public class Product : TrackedDocument
{
    public string Name { get; set; }

    public int OutletId { get; set; }
    public virtual Outlet Outlet { get; set; }

    public string Description { get; set; }

    public Inventory Inventory { get; set; }

    public string Unit { get; set; }

    public Pricing Price { get; set; }
}
namespace Domain;

public class Product : TrackedDocument
{
    public required string Name { get; set; }

    public int OutletId { get; set; }
    public virtual Outlet? Outlet { get; set; }

    public string? Description { get; set; }

    public required Inventory Inventory { get; set; }

    public required string Unit { get; set; }

    public required Pricing Price { get; set; }
}
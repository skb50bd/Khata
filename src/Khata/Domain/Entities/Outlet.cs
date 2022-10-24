namespace Domain;

public class Outlet : TrackedDocument
{
    public required string Title { get; set; }
    public required string TagLine { get; set; }
    public required string Address { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
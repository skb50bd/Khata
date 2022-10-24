
namespace Domain;

public abstract class Person : TrackedDocument
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string FullName => $"{ FirstName } { LastName }";
    public required string Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Note { get; set; }
}
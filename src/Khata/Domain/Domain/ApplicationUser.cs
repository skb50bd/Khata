using Microsoft.AspNetCore.Identity;

namespace Domain;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Role Role { get; set; }
    public byte[] Avatar { get; set; }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging.Abstractions;

namespace Data.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<KhataContext>
{
    private const string CONNECTION_STRING = "Data Source=/Users/shakibharis/Khata.db";
    
    public KhataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<KhataContext>();
        optionsBuilder.UseSqlite(CONNECTION_STRING);
        
        return new(
            optionsBuilder.Options, 
            NullLogger<KhataContext>.Instance
        );
    }
}
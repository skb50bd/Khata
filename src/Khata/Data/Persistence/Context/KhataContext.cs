using Data.Persistence.DbViews;

using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Data.Persistence;

public sealed class KhataContext : IdentityDbContext<ApplicationUser>
{
    public KhataContext(
            DbContextOptions<KhataContext> options,
            ILogger<KhataContext> logger
        ) : base(options)
    {
        var pm = Database.GetPendingMigrations();

        try
        {
            if (!pm.Any()) return;

            if (Database.ProviderName is "Microsoft.EntityFrameworkCore.SqlServer")
            {
                Database.CreateAllSQLServerViews();
            }

            // Database.Migrate();
        }
        catch (Exception e)
        {
            logger.LogError(
                "Could not apply Migrations "
                + JsonConvert.SerializeObject(pm));
            
            logger.LogError(e.Message);
        }

        // Database.EnsureCreated();
    }

    //public KhataContext(
    //    DbContextOptions<KhataContext> options)
    //    : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        var decimalColumns =
            builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal));
        
        foreach (var property in decimalColumns)
        {
            property.SetColumnType("decimal(18, 6)");
        }
        
        if (Database.ProviderName is "Microsoft.EntityFrameworkCore.SqlServer")
        {
            builder.BuildQueries(this);
        }
    }
}
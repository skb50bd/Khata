using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class OutletConfig : IEntityTypeConfiguration<Outlet>
{
    public void Configure(EntityTypeBuilder<Outlet> builder)
    {
        builder.Property(_ => _.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(_ => _.TagLine)
            .HasMaxLength(300);

        builder.Property(_ => _.Address)
            .HasMaxLength(500);

        builder.Property(_ => _.Phone)
            .HasMaxLength(50);

        builder.Property(_ => _.Email)
            .HasMaxLength(100);
        
        builder.HasMany(e => e.Sales)
            .WithOne(s => s.Outlet)
            .HasForeignKey(_ => _.OutletId);
        
        builder.HasMany(e => e.Products)
            .WithOne(s => s.Outlet)
            .HasForeignKey(_ => _.OutletId);
        
        builder.HasMany(e => e.Services)
            .WithOne(s => s.Outlet)
            .HasForeignKey(_ => _.OutletId);
    }
}
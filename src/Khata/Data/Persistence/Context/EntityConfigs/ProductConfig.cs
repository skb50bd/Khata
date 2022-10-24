using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(_ => _.Name)
            .HasMaxLength(200);

        builder.HasOne(_ => _.Outlet)
            .WithMany(_ => _.Products)
            .HasForeignKey(_ => _.OutletId)
            .IsRequired();

        builder.Property(_ => _.Description)
            .HasMaxLength(2000);

        builder.Property(_ => _.Unit)
            .HasMaxLength(50);
    }
}
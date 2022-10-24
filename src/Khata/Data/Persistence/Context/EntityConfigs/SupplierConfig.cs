using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class SupplierConfig : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasBaseType<Person>();
        
        builder.Property(_ => _.CompanyName)
            .HasMaxLength(200);

        builder.HasMany(_ => _.Purchases)
            .WithOne(_ => _.Supplier)
            .HasForeignKey(_ => _.SupplierId);
        
        builder.HasMany(_ => _.Payments)
            .WithOne(_ => _.Supplier)
            .HasForeignKey(_ => _.SupplierId);

        builder.HasMany(_ => _.PurchaseReturns)
            .WithOne(_ => _.Supplier)
            .HasForeignKey(_ => _.SupplierId);

        builder.HasIndex(_ => _.CompanyName);
    }
}
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class PurchaseConfig : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.HasOne(_ => _.Supplier)
            .WithMany(_ => _.Purchases)
            .HasForeignKey(_ => _.SupplierId)
            .IsRequired();

        builder.HasOne(p => p.Vouchar)
            .WithOne(v => v.Purchase)
            .HasForeignKey<Vouchar>(v => v.PurchaseId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasMany(_ => _.Cart)
            .WithOne(_ => _.Purchase)
            .HasForeignKey(_ => _.PurchaseId);

        builder.Navigation(_ => _.Cart)
            .AutoInclude();
        
        builder.Property(_ => _.Description)
            .HasMaxLength(2000);

        builder.OwnsOne(_ => _.Payment);
    }
}
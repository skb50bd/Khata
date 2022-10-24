using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class VoucharConfig : IEntityTypeConfiguration<Vouchar>
{
    public void Configure(EntityTypeBuilder<Vouchar> builder)
    {
        builder.HasBaseType<Invoice>();
        
        builder.HasOne(v => v.Purchase)
            .WithOne(p => p.Vouchar)
            .HasPrincipalKey<Vouchar>(_ => _.PurchaseId)
            .HasForeignKey<Purchase>(p => p.VoucharId);

        builder.HasOne(v => v.SupplierPayment)
            .WithOne(s => s.Vouchar)
            .HasForeignKey<SupplierPayment>(sp => sp.VoucharId)
            .HasPrincipalKey<Vouchar>(_ => _.SupplierPaymentId);

        builder.HasOne(_ => _.Supplier)
            .WithMany()
            .HasForeignKey(_ => _.SupplierId);
        
        builder.OwnsMany(s => s.Cart)
            .WithOwner(_ => _.Vouchar)
            .HasForeignKey(_ => _.VoucharId);

        builder.Navigation(_ => _.Cart)
            .AutoInclude();
    }
}
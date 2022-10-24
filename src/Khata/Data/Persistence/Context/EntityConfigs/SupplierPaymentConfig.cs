using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class SupplierPaymentConfig : IEntityTypeConfiguration<SupplierPayment>
{
    public void Configure(EntityTypeBuilder<SupplierPayment> builder)
    {
        builder.HasOne(_ => _.Supplier)
            .WithMany(_ => _.Payments)
            .HasForeignKey(_ => _.SupplierId)
            .IsRequired();
        
        builder.HasOne(sp => sp.Vouchar)
            .WithOne(v => v.SupplierPayment)
            .HasForeignKey<Vouchar>(v => v.SupplierPaymentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.Property(_ => _.Description)
            .HasMaxLength(2000);
    }
}
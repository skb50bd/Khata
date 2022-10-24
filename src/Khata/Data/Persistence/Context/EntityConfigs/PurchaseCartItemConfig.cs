using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class PurchaseCartItemConfig : IEntityTypeConfiguration<PurchaseCartItem>
{
    public void Configure(EntityTypeBuilder<PurchaseCartItem> builder)
    {
        builder.Property(_ => _.Name)
            .HasMaxLength(200);
        
        builder.HasOne(_ => _.Purchase)
            .WithMany(_ => _.Cart)
            .HasForeignKey(_ => _.PurchaseId)
            .IsRequired(false);

        builder.HasOne(_ => _.PurchaseReturn)
            .WithMany(_ => _.Cart)
            .HasForeignKey(_ => _.PurchaseReturnId)
            .IsRequired(false);

        builder.HasOne(_ => _.Product)
            .WithMany()
            .HasForeignKey(_ => _.ProductId);
    }
}
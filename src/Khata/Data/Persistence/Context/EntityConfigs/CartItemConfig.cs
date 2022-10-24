using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class CartItemConfig : IEntityTypeConfiguration<SaleCartItem>
{
    public void Configure(EntityTypeBuilder<SaleCartItem> builder)
    {
        builder.Property(_ => _.Name)
            .HasMaxLength(200);

        builder.HasOne(_ => _.Product)
            .WithMany()
            .HasForeignKey(_ => _.ProductId)
            .IsRequired(false);

        builder.HasOne(_ => _.Service)
            .WithMany()
            .HasForeignKey(_ => _.ServiceId)
            .IsRequired(false);

        builder.HasOne(_ => _.Sale)
            .WithMany(_ => _.Cart)
            .HasForeignKey(_ => _.SaleId)
            .IsRequired(false);

        builder.HasOne(_ => _.SavedSale)
            .WithMany(_ => _.Cart)
            .HasForeignKey(_ => _.SavedSaleId)
            .IsRequired(false);

        builder.HasOne(_ => _.Refund)
            .WithMany(_ => _.Cart)
            .HasForeignKey(_ => _.RefundId)
            .IsRequired(false);
    }
}
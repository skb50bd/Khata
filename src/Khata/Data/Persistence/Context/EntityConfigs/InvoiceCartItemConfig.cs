using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class InvoiceCartItemConfig : IEntityTypeConfiguration<InvoiceCartItem>
{
    public void Configure(EntityTypeBuilder<InvoiceCartItem> builder)
    {
        builder.Property(_ => _.Name)
            .HasMaxLength(200);

        builder.HasOne(_ => _.Invoice)
            .WithMany(_ => _.Cart)
            .HasForeignKey(_ => _.InvoiceId)
            .IsRequired(false);

        builder.HasOne(_ => _.Vouchar)
            .WithMany(_ => _.Cart)
            .HasForeignKey(_ => _.VoucharId)
            .IsRequired(false);
    }
}
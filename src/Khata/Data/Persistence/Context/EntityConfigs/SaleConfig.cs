using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class SaleConfig : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasOne(_ => _.Customer)
            .WithMany(_ => _.Purchases)
            .HasForeignKey(_ => _.CustomerId)
            .IsRequired();

        builder.HasOne(_ => _.Outlet)
            .WithMany(_ => _.Sales)
            .HasForeignKey(_ => _.OutletId)
            .IsRequired();

        builder.HasOne(_ => _.Invoice)
            .WithOne(_ => _.Sale)
            .HasForeignKey<CustomerInvoice>(_ => _.SaleId)
            .IsRequired();

        builder.HasMany(_ => _.Cart)
            .WithOne(_ => _.Sale)
            .HasForeignKey(_ => _.SaleId)
            .IsRequired(false);

        builder.Navigation(_ => _.Cart)
            .AutoInclude();

        builder.Ignore(_ => _.TableName);
        builder.Ignore(_ => _.RowId);
    }
}
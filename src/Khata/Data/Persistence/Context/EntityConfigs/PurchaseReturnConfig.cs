using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class PurchaseReturnConfig: IEntityTypeConfiguration<PurchaseReturn>
{
    public void Configure(EntityTypeBuilder<PurchaseReturn> builder)
    {
        builder.HasOne(_ => _.Supplier)
            .WithMany(_ => _.PurchaseReturns)
            .HasForeignKey(_ => _.SupplierId);

        builder.HasOne(_ => _.Purchase)
            .WithMany()
            .HasForeignKey(_ => _.PurchaseId);

        builder.HasMany(_ => _.Cart)
            .WithOne(_ => _.PurchaseReturn)
            .HasForeignKey(_ => _.PurchaseReturnId);

        builder.Navigation(_ => _.Cart)
            .AutoInclude();

        builder.Property(_ => _.Description)
            .HasMaxLength(2000);
    }
}
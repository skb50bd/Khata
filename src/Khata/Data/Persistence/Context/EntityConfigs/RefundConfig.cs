using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class RefundConfig : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.HasOne(_ => _.Customer)
            .WithMany(_ => _.Refunds)
            .HasForeignKey(_ => _.CustomerId);
        
        builder.HasOne(_ => _.Sale)
            .WithOne()
            .HasForeignKey<Refund>(r => r.SaleId);

        builder.HasMany(_ => _.Cart)
            .WithOne(_ => _.Refund)
            .HasForeignKey(_ => _.RefundId);

        builder.Navigation(_ => _.Cart)
            .AutoInclude();
        
        builder.Property(_ => _.Description)
            .HasMaxLength(2000);
    }
}
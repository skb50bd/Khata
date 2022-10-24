using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class DebtPaymentConfig : IEntityTypeConfiguration<DebtPayment>
{
    public void Configure(EntityTypeBuilder<DebtPayment> builder)
    {
        builder.HasOne(dp => dp.Customer)
            .WithMany(c => c.DebtPayments)
            .HasForeignKey(dp => dp.CustomerId)
            .IsRequired();
        
        builder.HasOne(dp => dp.Invoice)
            .WithOne(i => i.DebtPayment)
            .HasPrincipalKey<DebtPayment>(_ => _.Id)
            .HasForeignKey<CustomerInvoice>(ci => ci.DebtPaymentId)
            .IsRequired(false);

        builder.Property(_ => _.Description)
            .HasMaxLength(1000);

        builder.HasIndex(_ => _.CustomerId);
    }
}
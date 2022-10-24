using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class CustomerInvoiceConfig : IEntityTypeConfiguration<CustomerInvoice>
{
    public void Configure(EntityTypeBuilder<CustomerInvoice> builder)
    {
        builder.HasBaseType<Invoice>();

        builder.HasOne(i => i.Sale)
            .WithOne(s => s.Invoice)
            .HasForeignKey<Sale>(s => s.InvoiceId)
            .HasForeignKey<CustomerInvoice>(_ => _.SaleId)
            .IsRequired(false);

        builder.HasOne(i => i.DebtPayment)
            .WithOne(s => s.Invoice)
            .HasForeignKey<DebtPayment>(s => s.InvoiceId)
            .HasForeignKey<CustomerInvoice>(_ => _.DebtPaymentId)
            .IsRequired(false);

        builder.HasMany(s => s.Cart)
            .WithOne(_ => _.Invoice)
            .HasForeignKey(_ => _.InvoiceId);

        builder.Navigation(_ => _.Cart)
            .AutoInclude();
        
        builder.HasOne(_ => _.Outlet)
            .WithMany()
            .HasForeignKey(_ => _.OutletId)
            .IsRequired();

        builder.HasOne(_ => _.Customer)
            .WithMany()
            .HasForeignKey(_ => _.CustomerId)
            .IsRequired();
    }
}
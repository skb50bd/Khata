using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasBaseType<Person>();
        
        builder.Property(_ => _.CompanyName)
            .HasMaxLength(100);

        builder.HasMany(_ => _.Purchases)
            .WithOne(_ => _.Customer)
            .HasForeignKey(_ => _.CustomerId);

        builder.HasMany(_ => _.DebtPayments)
            .WithOne(_ => _.Customer)
            .HasForeignKey(_ => _.CustomerId);

        builder.HasMany(_ => _.Refunds)
            .WithOne(_ => _.Customer)
            .HasForeignKey(_ => _.CustomerId);
    }
}
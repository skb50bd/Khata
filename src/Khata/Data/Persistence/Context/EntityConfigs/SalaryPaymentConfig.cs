using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class SalaryPaymentConfig : IEntityTypeConfiguration<SalaryPayment>
{
    public void Configure(EntityTypeBuilder<SalaryPayment> builder)
    {
        builder.HasOne(_ => _.Employee)
            .WithMany(_ => _.SalaryPayments)
            .HasForeignKey(_ => _.EmployeeId);

        builder.Property(_ => _.Description)
            .HasMaxLength(2000);
    }
}
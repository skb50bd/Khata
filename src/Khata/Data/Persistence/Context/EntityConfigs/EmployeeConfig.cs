using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class EmployeeConfig : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasBaseType<Person>();
        
        builder.Property(_ => _.Designation)
            .HasMaxLength(100);

        builder.Property(_ => _.NIdNumber)
            .HasMaxLength(100);

        builder.HasIndex(_ => _.NIdNumber)
            .IsUnique();

        builder.HasMany(_ => _.SalaryIssues)
            .WithOne(_ => _.Employee)
            .HasForeignKey(_ => _.EmployeeId);

        builder.HasMany(_ => _.SalaryPayments)
            .WithOne(_ => _.Employee)
            .HasForeignKey(_ => _.EmployeeId);
    }
}
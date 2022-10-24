using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class SalaryIssueConfig : IEntityTypeConfiguration<SalaryIssue>
{
    public void Configure(EntityTypeBuilder<SalaryIssue> builder)
    {
        builder.HasOne(_ => _.Employee)
            .WithMany(_ => _.SalaryIssues)
            .HasForeignKey(_ => _.EmployeeId)
            .IsRequired();

        builder.Property(_ => _.Description)
            .HasMaxLength(2000);
    }
}
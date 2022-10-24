using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class PersonConfig : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(_ => _.FirstName)
            .HasMaxLength(100);

        builder.Property(_ => _.LastName)
            .HasMaxLength(100);

        builder.Property(_ => _.Address)
            .HasMaxLength(1000);

        builder.Property(_ => _.Email)
            .HasMaxLength(150);

        builder.Property(_ => _.Phone)
            .HasMaxLength(50);

        builder.Property(_ => _.Note)
            .HasMaxLength(2000);
        
        builder.HasIndex(_ => new
        {
            _.FirstName,
            _.LastName,
            _.Address,
            _.Email,
            _.Phone
        });
    }
}
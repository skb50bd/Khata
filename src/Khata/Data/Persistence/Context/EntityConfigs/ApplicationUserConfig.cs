using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(_ => _.FirstName)
            .HasMaxLength(100);

        builder.Property(_ => _.LastName)
            .HasMaxLength(100);

        builder.Property(_ => _.Avatar);
    }
}
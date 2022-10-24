using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class ServiceConfig : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(_ => _.Name)
            .HasMaxLength(200);

        builder.Property(_ => _.Description)
            .HasMaxLength(2000);

        builder.HasOne(_ => _.Outlet)
            .WithMany(_ => _.Services)
            .HasForeignKey(_ => _.OutletId)
            .IsRequired();
    }
}
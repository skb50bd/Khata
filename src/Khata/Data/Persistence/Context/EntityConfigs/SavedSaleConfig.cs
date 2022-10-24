using Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistence.Context.EntityConfigs;

public class SavedSaleConfig : IEntityTypeConfiguration<SavedSale>
{
    public void Configure(EntityTypeBuilder<SavedSale> builder)
    {
        builder.HasOne(_ => _.Customer)
            .WithMany()
            .HasForeignKey(_ => _.CustomerId)
            .IsRequired();

        builder.HasOne(_ => _.Outlet)
            .WithMany()
            .HasForeignKey(_ => _.OutletId)
            .IsRequired();

        builder.HasMany(_ => _.Cart)
            .WithOne(_ => _.SavedSale)
            .HasForeignKey(_ => _.SavedSaleId)
            .IsRequired(false);

        builder.Navigation(_ => _.Cart)
            .AutoInclude();

        builder.Property(_ => _.Description)
            .HasMaxLength(2000);
    }
}
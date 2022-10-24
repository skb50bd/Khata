using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Data.Persistence.Context.EntityConfigs;

public class CurrentDateTimeOffsetValueGenerator: ValueGenerator<DateTimeOffset>
{
    private readonly IDateTimeProvider _dateTime;
    public CurrentDateTimeOffsetValueGenerator(IDateTimeProvider dateTime) => _dateTime = dateTime;
    public override DateTimeOffset Next(EntityEntry entry) => _dateTime.Now;
    public override bool GeneratesTemporaryValues => false;
}

public class CurrentUserNameValueGenerator : ValueGenerator<string>
{
    // private readonly IAuthService _authService;
    public override string Next(EntityEntry entry)
    {
        // return _authService.GetCurrentUserName().Result;
        // TODO: Add a Proper AuthService
        throw new NotImplementedException();
    }

    public override bool GeneratesTemporaryValues => false;
}

public class MetadataConfig : IEntityTypeConfiguration<Metadata>
{
    public void Configure(EntityTypeBuilder<Metadata> builder)
    {
        builder.Metadata.IsOwned();

        builder.HasNoKey();
        
        builder.Property(_ => _.Creator)
            .HasMaxLength(200);

        builder.Property(_ => _.Modifier)
            .HasMaxLength(200);

        builder.Property(_ => _.CreationTime)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CurrentDateTimeOffsetValueGenerator>();

        builder.Property(_ => _.Creator)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<CurrentUserNameValueGenerator>();
            
        builder.Property(_ => _.ModificationTime)
            .ValueGeneratedOnAddOrUpdate()
            .HasValueGenerator<CurrentDateTimeOffsetValueGenerator>();
        
        builder.Property(_ => _.Modifier)
            .ValueGeneratedOnAddOrUpdate()
            .HasValueGenerator<CurrentUserNameValueGenerator>();
    }
}
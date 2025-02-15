using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using LunaSphere.Domain.RefreshTokens;

namespace LunaSphere.Infrastructure.RefreshTokens.Persistence;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Define the primary key and set as integer GENERATED ALWAYS AS IDENTITY
        builder.Property(prop => prop.Id).UseIdentityAlwaysColumn();

        // Define a unique index on the Token property
        builder.HasIndex(x => x.Token).IsUnique();

        // Apply query filter for isActive property
        builder.HasQueryFilter(x => x.IsActive == true);

        builder.Property(prop => prop.Token)
            .IsUnicode(false)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(prop => prop.ExperiesAt)
            .IsRequired();

        builder.Property(prop => prop.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
        
        builder.Property(prop => prop.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();
    }
}
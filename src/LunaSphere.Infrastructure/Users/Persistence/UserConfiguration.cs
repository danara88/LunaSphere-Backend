using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using LunaSphere.Domain.Users.Enums;
using LunaSphere.Domain.Users;
using LunaSphere.Domain.RefreshTokens;

namespace LunaSphere.Infrastructure.Users.Persistence;

/// <summary>
/// Sets user table configurations
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Define the primary key and set as integer GENERATED ALWAYS AS IDENTITY
        builder.Property(prop => prop.Id).UseIdentityAlwaysColumn();

        // Define a unique index on the Email property
        builder.HasIndex(x => x.Email).IsUnique();

        // Apply query filter for isActive property
        builder.HasQueryFilter(x => x.IsActive == true);

        // Apply one to one relationship with RefreshToken table
        builder.HasOne(u => u.RefreshToken)
            .WithOne(r => r.User)
            .HasForeignKey<RefreshToken>(r => r.UserId);

        builder.Property(prop => prop.FirstName)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(prop => prop.LastName)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(prop => prop.Email)
            .IsUnicode(false)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(prop => prop.PasswordHash)
            .IsUnicode(false)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(prop => prop.Role)
            .HasDefaultValue(RoleType.standard)
            .IsRequired();

        builder.Property(prop => prop.IsGoogle)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(prop => prop.LastLogin)
            .IsRequired(false);

        builder.Property(prop => prop.VerificationToken)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(prop => prop.VerifiedAt)
            .IsRequired(false);

        builder.Property(prop => prop.PasswordResetToken)
            .IsUnicode(false)
            .IsRequired(false);
        
        builder.Property(prop => prop.PasswordResetTokenExpires)
            .IsRequired(false);

        builder.Property(prop => prop.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
        
        builder.Property(prop => prop.CreatedAt)
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();
    }
}
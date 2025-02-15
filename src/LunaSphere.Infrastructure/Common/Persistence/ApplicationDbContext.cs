using Microsoft.EntityFrameworkCore;
using System.Reflection;

using LunaSphere.Domain.Users;
using LunaSphere.Domain.RefreshTokens;

namespace LunaSphere.Infrastructure.Common.Persistence;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Represents User Table
    /// </summary>
    public DbSet<User> User { get; set; }

    /// <summary>
    /// Represents RefreshToken Table
    /// </summary>
    public DbSet<RefreshToken> RefreshToken { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Uncomment if all tables will have id property as integer GENERATED ALWAYS AS IDENTITY
        //modelBuilder.UseIdentityByDefaultColumns();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
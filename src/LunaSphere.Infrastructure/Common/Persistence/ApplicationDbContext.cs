using Microsoft.EntityFrameworkCore;
using System.Reflection;

using LunaSphere.Domain.Users;

namespace LunaSphere.Infrastructure.Common.Persistence;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Represents User Table
    /// </summary>
    public required DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Uncomment if all tables will have id property as integer GENERATED ALWAYS AS IDENTITY
        //modelBuilder.UseIdentityByDefaultColumns();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
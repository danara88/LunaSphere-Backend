using Microsoft.EntityFrameworkCore;
using System.Reflection;

using LunaSphere.Domain.Users;
using LunaSphere.Domain.RefreshTokens;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Application.Users.Interfaces;
using LunaSphere.Application.RefreshTokens.Interfaces;
using Microsoft.AspNetCore.Http;
using MediatR;
using LunaSphere.Infrastructure.Users.Persistence;
using LunaSphere.Infrastructure.RefreshTokens.Persistence;
using LunaSphere.Domain.Common;
using LunaSphere.Domain.Common.Interfaces;

namespace LunaSphere.Infrastructure.Common.Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    /// <summary>
    /// Represents User Table
    /// </summary>
    public DbSet<User> User { get; set; }

    /// <summary>
    /// Represents RefreshToken Table
    /// </summary>
    public DbSet<RefreshToken> RefreshToken { get; set; }

    private readonly IUserRepository? _userRepository;
    private readonly IRefreshTokenRepository? _refreshTokenRepository;
    

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublisher _publisher;
    public IUserRepository UserRepository => _userRepository ?? new UserRepository(this);
    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ?? new RefreshTokenRepository(this);

    public ApplicationDbContext
    (
        DbContextOptions options,
        IHttpContextAccessor httpContextAccessor,
        IPublisher publisher
    ) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _publisher = publisher;
    }

    public bool HasChanges() => ChangeTracker.HasChanges();

    public async Task CommitChangesAsync()
    {
        // Get all the domain events
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        // Store domain events in the http context for later if user is waiting online
        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
        }
        else
        {
            await PublishDomainEvents(domainEvents);
        }

        await SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Uncomment if all tables will have id property as integer GENERATED ALWAYS AS IDENTITY
        //modelBuilder.UseIdentityByDefaultColumns();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        // Fetch queue from http context or create a new queue if it doen't exist
        var domainEventsQueue = _httpContextAccessor.HttpContext!.Items
            .TryGetValue("DomainEventsQueue", out var value)
            && value is Queue<IDomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new Queue<IDomainEvent>();

        // Add the domain events to the end of the domain events queue
        domainEvents.ForEach(domainEventsQueue.Enqueue);

        // Store the domain events queue in the http context
        _httpContextAccessor.HttpContext.Items["DomainEventsQueue"] = domainEventsQueue;
    }

   
}
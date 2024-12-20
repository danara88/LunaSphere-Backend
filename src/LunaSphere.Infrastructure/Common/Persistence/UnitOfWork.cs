using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Application.Users.Interfaces;
using LunaSphere.Infrastructure.Users.Persistence;

namespace LunaSphere.Infrastructure.Common.Persistence;

/// <summary>
/// Unit Of Work design patter:
/// Wraps multiple database changes into a single transaction.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository? _userRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

    public bool HasChanges()
    {
       return _context.ChangeTracker.HasChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
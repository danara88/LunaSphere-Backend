using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Application.RefreshTokens.Interfaces;
using LunaSphere.Application.Users.Interfaces;

namespace LunaSphere.Application.Common.Interfaces;

/// <summary>
/// Unit of work interface
/// </summary>
public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    bool HasChanges();
    Task SaveChangesAsync();
}
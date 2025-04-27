using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.RefreshTokens;

namespace LunaSphere.Application.RefreshTokens.Interfaces;

/// <summary>
/// Represents the refresh token repository interface
/// </summary>
public interface IRefreshTokenRepository : IBaseRepository<RefreshToken> 
{
    Task<RefreshToken?> GetByTokenAsync(string token);

    Task<RefreshToken?> GetByUserIdAsync(int userId);
}
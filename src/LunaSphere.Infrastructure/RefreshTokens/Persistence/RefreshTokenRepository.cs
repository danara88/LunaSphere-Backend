using Microsoft.EntityFrameworkCore;

using LunaSphere.Application.RefreshTokens.Interfaces;
using LunaSphere.Domain.RefreshTokens;
using LunaSphere.Infrastructure.Common.Persistence;

namespace LunaSphere.Infrastructure.RefreshTokens.Persistence;

public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Method to find a refresh token item by token
    /// </summary>
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _entity.Include(r => r.User)
                            .FirstOrDefaultAsync(r => r.Token == token);
    }

    public async Task<RefreshToken?> GetByUserIdAsync(int userId)
    {
        return await _entity.Include(r => r.User)
                            .FirstOrDefaultAsync(r => r.UserId == userId);
    }
}
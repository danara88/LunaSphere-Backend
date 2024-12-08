using Microsoft.EntityFrameworkCore;

using LunaSphere.Application.Users.Interfaces;
using LunaSphere.Domain.Users;
using LunaSphere.Infrastructure.Common.Persistence;

namespace LunaSphere.Infrastructure.Users.Persistence;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    /// <summary>
    /// Method to check if a user exists by email
    /// </summary>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
       return await _entity.AnyAsync(u => u.Email == email);
    }

    /// <summary>
    /// Method to find a user by email address
    /// </summary>
    public async Task<User> GetByEmailAsync(string email)
    {
        return await _entity.FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <summary>
    /// Method to find a user by verification token 
    /// </summary>
    public async Task<User> GetByVerificationTokenAsync(string verificationToken)
    {
        return await _entity.FirstOrDefaultAsync(u => u.VerificationToken == verificationToken);
    }
}
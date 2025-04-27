using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users;

namespace LunaSphere.Application.Users.Interfaces;

/// <summary>
/// Represents the user repository interface
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email);

    Task<bool> ExistsByEmailAsync(string email);

    Task<User?> GetByVerificationCodeAsync(short verificationCode);

    Task<User?> GetByVerificationTokenAsync(string verificationToken);
}
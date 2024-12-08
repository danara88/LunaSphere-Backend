using LunaSphere.Domain.Users;

namespace LunaSphere.Application.Auth.Interfaces;

/// <summary>
/// JWT Factory interface
/// </summary>
public interface IJwtFactory
{
    string GenerateJwtToken(User user);
}

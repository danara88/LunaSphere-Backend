namespace LunaSphere.Application.Auth.Interfaces;

/// <summary>
/// Refresh Token Factory interface
/// </summary>
public interface IRefreshTokenFactory
{
    string GenerateRefreshToken();
}
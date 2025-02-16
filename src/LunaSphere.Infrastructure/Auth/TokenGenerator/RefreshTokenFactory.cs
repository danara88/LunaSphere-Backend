using System.Security.Cryptography;

using LunaSphere.Application.Auth.Interfaces;

namespace LunaSphere.Infrastructure.Auth.TokenGenerator;

public class RefreshTokenFactory : IRefreshTokenFactory
{   
    /// <summary>
    /// Generates secure refresh token randomly.
    /// Creates an array of bytes with a cryptographically strong random sequence.
    /// </summary>
    public string GenerateRefreshToken() 
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
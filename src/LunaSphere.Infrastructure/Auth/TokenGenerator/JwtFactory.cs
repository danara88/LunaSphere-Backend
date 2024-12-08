using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Domain.Users;

namespace LunaSphere.Infrastructure.Auth.TokenGenerator;

/// <summary>
/// JWT Factory implementation
/// </summary>
public class JwtFactory : IJwtFactory
{
    private readonly JwtSettings _jwtSettings;

    public JwtFactory(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Method to generate the JWT token
    /// </summary>
    public string GenerateJwtToken(User user)
    {
        JwtSecurityTokenHandler jwtTokenHandler = new ();
        byte[] secretKey = Encoding.UTF8.GetBytes(_jwtSettings.Secret!);

        List<Claim> claims = new()
        {
            new Claim("id", user.Id.ToString()),    
            new Claim("isGoogle", user.IsGoogle.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),

            // Include Iat (Issued at) identify the date and time when this token was emitted.
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

            // Include Jti (token ID) to avoid client re-use the token again.
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if(user.FirstName is not null)
        {
            claims.Add(new Claim(ClaimTypes.Name, user.FirstName));
        }

        if(user.LastName is not null)
        {
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ValidTimeMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,

            // Verified that the secret key is issued by the ORIGINAL source.
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
        };

        SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return jwtTokenHandler.WriteToken(token);
    }
}


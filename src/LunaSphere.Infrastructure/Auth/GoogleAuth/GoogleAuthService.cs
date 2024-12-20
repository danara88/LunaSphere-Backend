using Microsoft.Extensions.Options;
using Google.Apis.Auth;
using ErrorOr;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Application.Auth.DTOs;
using LunaSphere.Domain.Users;

namespace LunaSphere.Infrastructure.Auth.GoogleAuth;

public class GoogleAuthService : IGoogleAuthService
{   
    private readonly GoogleAuthSettings _googleAuthSettings;

    public GoogleAuthService(IOptions<GoogleAuthSettings> googleAuthSettings)
    {
        _googleAuthSettings = googleAuthSettings.Value;
    }

    public async Task<ErrorOr<GoogleAuthRespDTO>> VerifyGoogleTokenAsync(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [_googleAuthSettings.GoogleId]
        };

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            
            return new GoogleAuthRespDTO
            (
                Name: payload.Name,
                FamilyName: payload.FamilyName,
                Email: payload.Email,
                Picture: payload.Picture
            );
            
        }
        catch (InvalidJwtException)
        {
            return AuthErrors.GoogleInvalidJwtToken;
        }
        catch (Exception)
        {
            return AuthErrors.GoogleAuthFailure;
        }
    }
}
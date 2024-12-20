using ErrorOr;

using LunaSphere.Application.Auth.DTOs;

namespace LunaSphere.Application.Common.Interfaces;

public interface IGoogleAuthService
{
    public Task<ErrorOr<GoogleAuthRespDTO>> VerifyGoogleTokenAsync(string token);
}
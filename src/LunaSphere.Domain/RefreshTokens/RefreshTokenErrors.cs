using ErrorOr;

namespace LunaSphere.Domain.RefreshTokens;

/// <summary>
/// Refresh token errors
/// </summary>

public static class RefreshTokenErrors
{
    public static readonly Error RefreshTokenExpired = Error.Validation(
        code: "RefreshTokenExpired",
        description: "Your refresh token has expired. Please re-authenticate to obtain a new one."
    );

     public static readonly Error RefreshTokenUnAuthorized = Error.Unauthorized(
        code: "RefreshTokenUnAuthorized",
        description: "You are not allowed to perform this action."
    );
}
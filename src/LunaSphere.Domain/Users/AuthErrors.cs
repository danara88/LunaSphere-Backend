using ErrorOr;

namespace LunaSphere.Domain.Users;

/// <summary>
/// Auth errors
/// </summary>
public static class AuthErrors
{
   public static readonly Error InvalidCredentials = Error.Unauthorized(
        code: "Auth.InvalidCredentials",
        description: "Your email account or password is incorrect.");

    public static readonly Error WeakPassword = Error.Validation(
        code: "Auth.WeakPassword",
        description: "Password too weak. Password must be 8+ characters, with uppercase, lowercase, a number, and a special character.");

    public static readonly Error UserAlreadyRegistered = Error.Conflict(
        code: "Auth.UserAlreadyRegistered",
        description: "This account already exists. Enter a different account or request a new one.");

    public static readonly Error UserAccountNotVerified = Error.Validation(
        code: "Auth.UserAccountNotVerified",
        description: "Your account is not verified yet. Please check your email inbox for the verification link and confirm your account."
    );

    public static readonly Error InvalidVerificationToken = Error.Validation(
        code: "Auth.InvalidVerificationToken",
        description: "An error occurred while verifying your account."
    );
}
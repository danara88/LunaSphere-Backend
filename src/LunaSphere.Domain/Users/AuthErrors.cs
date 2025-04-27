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

     public static readonly Error UserAccountAlreadyVerified = Error.Validation(
        code: "Auth.UserAccountAlreadyVerified",
        description: "Your account has already been verified. No further action is required."
    );

    public static readonly Error InvalidVerificationToken = Error.Validation(
        code: "Auth.InvalidVerificationToken",
        description: "An error occurred while verifying your account. Please sign in again."
    );

    public static readonly Error VerificationCodeNotExpiredYet = Error.Validation(
        code: "Auth.VerificationCodeNotExpiredYet",
        description: "The verification code is still valid and has not expired yet. Please wait before requesting a new one."
    );

    public static readonly Error VerificationCodeExpired = Error.Validation(
        code: "Auth.VerificationCodeExpired",
        description: "The verification code has expired. Please request a new one."
    );

    public static readonly Error InvalidVerificationCode = Error.Validation(
        code: "Auth.InvalidVerificationCode",
        description: "The verification code is not valid."
    );

    public static readonly Error NoVerificationCodeAssigned = Error.Validation(
        code: "Auth.NoVerificationCodeAssigned",
        description: "No verification code has been assigned yet."
    );

    public static readonly Error UserForbiddenAction = Error.Forbidden(
        code: "Auth.UserForbiddenAction",
        description: "You don't have permissions to perform this action."
    );

    public static readonly Error UserIsNotAuthenticated = Error.Unauthorized(
        code: "Auth.UserIsNotAuthenticated",
        description: "Authentication is required to perform this action."
    );

    public static readonly Error GoogleInvalidJwtToken = Error.Unauthorized(
        code: "Auth.GoogleInvalidJwtToken",
        description: "Google authentication failed because the provided token is invalid."
    );

    public static readonly Error GoogleAuthFailure = Error.Failure(
        code: "Auth.GoogleAuthFailure",
        description: "An unexpected error occurred during google authentication."
    );
}
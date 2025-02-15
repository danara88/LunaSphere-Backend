using LunaSphere.Domain.Common;
using LunaSphere.Domain.Users.Enums;
using LunaSphere.Domain.RefreshTokens;

namespace LunaSphere.Domain.Users;

/// <summary>
/// Domain entity representing a User
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// User's first name.
    /// This property can be null at the begining.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// User's last name.
    /// This property can be null at the begining.
    /// </summary>    
    public string? LastName { get; set; }

    /// <summary>
    /// User's email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's hashed password
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if user was register with Google auth provider
    /// </summary>
    public bool IsGoogle { get; set; } = false;

    /// <summary>
    /// User's role
    /// </summary>
    public RoleType Role { get; set; } = RoleType.standard;

    /// <summary>
    /// Date and time when the user logged in
    /// </summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// Token for verifying the user's account
    /// </summary>        
    public string? VerificationToken { get; set; }

    /// <summary>
    /// Date and time when the verification token expires
    /// </summary>        
    public DateTime? VerificationTokenExpires { get; set; }

    /// <summary>
    /// Date and time when the user verified their account via email
    /// </summary>
    public DateTime? VerifiedAt { get; set; }

    /// <summary>
    /// Random token to allow the user to reset their password
    /// </summary>
    public string? PasswordResetToken { get; set; }

    /// <summary>
    /// Date and time when the password reset token expires
    /// </summary>
    public DateTime? PasswordResetTokenExpires { get; set; }

    /// <summary>
    /// Refresh Token navigation property
    /// </summary>
    public virtual RefreshToken RefreshToken { get; set; }
}
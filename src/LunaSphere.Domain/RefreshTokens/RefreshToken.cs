using LunaSphere.Domain.Common;
using LunaSphere.Domain.Users;

namespace LunaSphere.Domain.RefreshTokens;

/// <summary>
/// Domain entity representing a RefreshToken
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// Represents the user's ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Represents the refresh token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Indicates the expiration date and time for the refresh token
    /// </summary>
    public DateTime ExperiesAt { get; set; }

    /// <summary>
    /// User navigation property
    /// </summary>
    public virtual User User { get; set; }
}
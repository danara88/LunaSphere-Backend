namespace LunaSphere.Infrastructure.Auth.GoogleAuth;

/// <summary>
/// Represents settings for configure OAuth2.0 with Google
/// </summary>
public class GoogleAuthSettings
{
    /// <summary>
    /// Google ID
    /// </summary>
    public string GoogleId { get; set; } = string.Empty;

    /// <summary>
    /// Google Sceret
    /// </summary>
    public string Secret { get; set; } = string.Empty;
}
namespace LunaSphere.Infrastructure.Auth.TokenGenerator;

/// <summary>
/// JWT settings
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// JWT secret key
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// JWT token valid time in minutes
    /// </summary>
    public int ValidTimeMinutes { get; set; }

    /// <summary>
    /// Source that emits the token
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Valid audience.
    /// Change this to the client's source
    /// </summary>
    public string Audience { get; set; } = string.Empty;
}

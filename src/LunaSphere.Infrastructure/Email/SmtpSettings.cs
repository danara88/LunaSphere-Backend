namespace LunaSphere.Infrastructure.Email;

/// <summary>
/// SMTP settings to send emails
/// </summary>
public class SmtpSettings
{
    public string Server { get; set; } = string.Empty;

    public int Port { get; set; }

    public string SenderEmail { get; set; } = string.Empty;

    public string SenderName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
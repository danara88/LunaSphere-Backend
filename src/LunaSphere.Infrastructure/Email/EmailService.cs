using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Users;

namespace LunaSphere.Infrastructure.Email;

/// <summary>
/// Service in charge to send emails
/// </summary>
public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly IWebHostEnvironment _hostEnvironment;

    public EmailService(IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment hostEnvironment)
    {
        _smtpSettings = smtpSettings.Value;
        _hostEnvironment = hostEnvironment;
    }

    /// <summary>
    /// Method to send an email
    /// </summary>
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var message = new MimeMessage();

            // From
            message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));

            // To
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlMessage };

            // Open smtp client and send email
            using (var client = new SmtpClient())
            {
                // StartTls: Inform to mail server that the content must be encrypted
                // Use 587 port to send encrypted mail
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port,  SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }
    
    /// <summary>
    /// In charge of sending a verification email template
    /// </summary>
    public async Task SendVerificationEmail(User user)
    {
         var pathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
            + "Templates" + Path.DirectorySeparatorChar.ToString()
            + Path.DirectorySeparatorChar.ToString() + "Verification_Code_Email.html";

        var htmlBody = "";
        using (StreamReader streamReader = File.OpenText(pathToFile))
        {
            htmlBody = streamReader.ReadToEnd();
        }

        string messageBody = string.Format(htmlBody, user.VerificationCode);

        await SendEmailAsync(user.Email, "LunaSphere. Verification Code.", messageBody);
    }
}

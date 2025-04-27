using Microsoft.AspNetCore.Identity.UI.Services;

using LunaSphere.Domain.Users;

namespace LunaSphere.Application.Common.Interfaces;

public interface IEmailService : IEmailSender
{
    Task SendVerificationEmail(User user);
}
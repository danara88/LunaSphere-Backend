
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Infrastructure.Common.Persistence;
using LunaSphere.Infrastructure.Auth.TokenGenerator;
using LunaSphere.Infrastructure.Auth.PasswordHasher;
using LunaSphere.Infrastructure.Auth.GoogleAuth;
using LunaSphere.Infrastructure.Email;
using LunaSphere.Application.Common.Helpers;


namespace LunaSphere.Infrastructure;

/// <summary>
/// Dependency injection module for infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // JWT configuration
        services.AddTransient<IJwtFactory, JwtFactory>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Refresh Token configuration
        services.AddTransient<IRefreshTokenFactory, RefreshTokenFactory>();

        // Google auth configuration
        services.Configure<GoogleAuthSettings>(configuration.GetSection("GoogleAuthSettings"));
        services.AddTransient<IGoogleAuthService, GoogleAuthService>();

        // Mail configuration
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.AddScoped<IEmailService, EmailService>();

        // Security helper configuration
        services.Configure<SecuritySettings>(configuration.GetSection("SecuritySettings"));
        services.AddScoped<ISecurityHelper, SecurityHelper>();

        return services;
    }
}
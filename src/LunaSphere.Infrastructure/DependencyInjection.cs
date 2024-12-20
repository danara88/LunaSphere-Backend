
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using LunaSphere.Application.Auth.Interfaces;
using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Infrastructure.Common.Persistence;
using LunaSphere.Infrastructure.Auth.TokenGenerator;
using LunaSphere.Infrastructure.Auth.PasswordHasher;
using LunaSphere.Infrastructure.Auth.GoogleAuth;


namespace LunaSphere.Infrastructure;

/// <summary>
/// Dependency injection module for infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // JWT configuration
        services.AddTransient<IJwtFactory, JwtFactory>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Google auth configuration
        services.Configure<GoogleAuthSettings>(configuration.GetSection("GoogleAuthSettings"));
        services.AddTransient<IGoogleAuthService, GoogleAuthService>();

        return services;
    }
}
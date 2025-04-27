using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using System.Text;

using LunaSphere.Infrastructure.Common.Persistence;
using LunaSphere.Infrastructure.Filters;
using LunaSphere.Api.Providers;
using LunaSphere.Application.Users.Interfaces;

namespace LunaSphere.Api;

/// <summary>
/// Dependency Injection Module for Api Layer
/// </summary>
public static class DependencyInjection 
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureControllers();
        services.ConfigureDatabase(configuration);
        services.ConfigureAuthentication(configuration);
        services.ConfigureDataProtection();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();
        services.AddHttpContextAccessor();
        services.AddCorsPolicy();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        
        return services;
    }

    private static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(options => 
        {
            options.Filters.Add<GlobalExceptionFilter>();
        });

        return services;
    }

    private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultDbConnection")));
        
        return services;
    }

    private static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwt =>
        {
            byte[] secretKey = Encoding.ASCII.GetBytes(configuration.GetSection("JwtSettings:Secret").Value!);

            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters()
            {
                // Always validate the secret key that is on theMicrosoft.AspNetCore.Authentication.JwtBearer --version 8.0.11 token
                ValidateIssuerSigningKey = true,

                // The key that we get must be equal to the key that we have created in the issuer
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                // Change to true in production envs.
                // It validates that the ORIGINAL issuer is the one that emit the token.
                // Ensures that there is not another source that emit the token.
                ValidateIssuer = true,
                ValidIssuer = configuration["JwtSettings:Issuer"],

                // Change to true in production envs.
                // If the client (audience) receive the token, that client can not re-use it in any other place.
                ValidateAudience = true,
                ValidAudience = configuration["JwtSettings:Audience"],

                // Sets token expiration time
                RequireExpirationTime = true,

                // Validates the life time of the token
                ValidateLifetime = true
            };
        });

        return services;
    }

    private static IServiceCollection AddCorsPolicy(this IServiceCollection services) 
    {
        services.AddCors(options => options.AddPolicy("AppCorsPolicy", build => 
        {
            build
                .WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        }));
        return services;
    }

    private static IServiceCollection ConfigureDataProtection(this IServiceCollection services)
    {
        // TODO: Imrpove data protection levels adding PersistKeysToFileSystem and ProtectKeysWithCertificate
        services.AddDataProtection()
            .SetApplicationName("LunaSphere");

        return services;
    }
}
using FluentValidation;
using LunaSphere.Application.Common.Behaviors;

using Microsoft.Extensions.DependencyInjection;

namespace LunaSphere.Application;

/// <summary>
/// Dependency Injection Module for Application Layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add and configure AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddMediatR(options =>
        {
            // Add commands/queries from the current assembly (Application layer)
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            // Add generic behavior for validation
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register all the validators IValidator<T> from the current assembly (Application layer)
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));


        return services;
    }
}
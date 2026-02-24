using FileManager.Extensions;
using AlAshmar.Infrastructure.Services;
using AlAshmar.Application.Interfaces;
using AlAshmar.Infrastructure.Persistence;

namespace AlAshmar.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services.
/// </summary>
public static class InfrastructureServiceRegistration
{
    /// <summary>
    /// Adds infrastructure services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register File Manager Services
        services.AddFileManager();

        // Register HttpContext Service Manager
        services.AddScoped<IHttpContextServiceManager, HttpContextServiceManager>();

        // Register Token Service (needs DbContext for permissions)
        services.AddScoped<ITokenService, TokenService>();

        // Register Authentication Service
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // Register Authorization Service
        services.AddScoped<Application.Interfaces.IAuthorizationService, AuthorizationService>();

        return services;
    }

    /// <summary>
    /// Adds authorization policies for the application.
    /// </summary>
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization();
        return services;
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AlAshmar.Application.Repos;
using AlAshmar.Infrastructure.Persistence.Repos;

namespace AlAshmar.Infrastructure.Persistence;

/// <summary>
/// Extension methods for registering persistence services.
/// </summary>
public static class PersistenceServiceRegistration
{
    /// <summary>
    /// Adds persistence services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // Register Repository
        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));

        // Register Unit of Work
        services.AddScoped<AlAshmar.Infrastructure.Persistence.UnitOfWork.IUnitOfWork, AlAshmar.Infrastructure.Persistence.UnitOfWork.UnitOfWork>();

        return services;
    }
}

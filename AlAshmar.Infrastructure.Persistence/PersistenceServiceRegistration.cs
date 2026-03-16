using AlAshmar.Application.Repos;
using AlAshmar.Infrastructure.Persistence.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlAshmar.Infrastructure.Persistence;

public static class PersistenceServiceRegistration
{

    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));

        services.AddScoped<ITokenRepository, Repos.TokenRepository>();

        services.AddScoped<AlAshmar.Infrastructure.Persistence.UnitOfWork.IUnitOfWork, AlAshmar.Infrastructure.Persistence.UnitOfWork.UnitOfWork>();

        return services;
    }
}

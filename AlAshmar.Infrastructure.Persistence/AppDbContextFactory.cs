using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AlAshmar.Infrastructure.Persistence;

/// <summary>
/// Design-time factory for creating AppDbContext instances for EF Core tools.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Use a default connection string for design-time migrations
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=AlAshmar;Trusted_Connection=True;";
        
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("AlAshmar.Infrastructure.Persistence"));
        
        return new AppDbContext(optionsBuilder.Options);
    }
}

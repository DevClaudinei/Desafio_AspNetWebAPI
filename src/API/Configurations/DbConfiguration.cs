using EntityFrameworkCore.UnitOfWork.Extensions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configurations;

public static class DbConfiguration
{
    public static void AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
        {
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                            ServerVersion.Parse("8.0.29-mysql"),
                            b => b.MigrationsAssembly("Infrastructure.Data"));
        }, contextLifetime: ServiceLifetime.Transient);
        services.AddUnitOfWork<ApplicationDbContext>(ServiceLifetime.Transient);
    }
}
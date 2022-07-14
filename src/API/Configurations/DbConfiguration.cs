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
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(connectionString,
                            ServerVersion.Parse("8.0.29-mysql"),
                            b => b.MigrationsAssembly("Infrastructure.Data"));
        });
    }
}

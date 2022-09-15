using AppServices.Services;
using AppServices.Services.Interfaces;
using DomainServices;
using DomainServices.Services;
using DomainServices.Services.Interfaces;
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
        });

        services.AddUnitOfWork(ServiceLifetime.Transient);

        services.AddTransient<ICustomerService, CustomerService>();
        
        services.AddTransient<ICustomerAppService, CustomerAppService>();

        services.AddTransient<ICustomerBankInfoService, CustomerBankInfoService>();

        services.AddTransient<ICustomerBankInfoAppService, CustomerBankInfoAppService>();

        services.AddTransient<IProductService, ProductService>();

        services.AddTransient<IProductAppService, ProductAppService>();

        services.AddTransient<IPortfolioService, PortfolioService>();

        services.AddTransient<IPortfolioAppService, PortfolioAppService>();

        services.AddTransient<IPortfolioProductService, PortfolioProductService>();

        services.AddTransient<IPortfolioProductAppService, PortfolioProductAppService>();

        services.AddTransient<DbContext, ApplicationDbContext>();
    }
}

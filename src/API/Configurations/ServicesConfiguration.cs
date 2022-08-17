using AppServices.Services;
using AppServices.Services.Interfaces;
using DomainServices;
using DomainServices.Services;
using DomainServices.Services.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configurations;

public static class ServicesConfiguration
{
    public static void AddServicesConfiguration(this IServiceCollection services)
    {
        services.AddTransient<ICustomerService, CustomerService>();

        services.AddTransient<ICustomerBankInfoService, CustomerBankInfoService>();

        services.AddTransient<IProductService, ProductService>();

        services.AddTransient<IPortfolioService, PortfolioService>();

        services.AddTransient<ICustomerAppService, CustomerAppService>();

        services.AddTransient<ICustomerBankInfoAppService, CustomerBankInfoAppService>();

        services.AddTransient<IProductAppService, ProductAppService>();

        services.AddTransient<IPortfolioAppService, PortfolioAppService>();

        services.AddTransient<DbContext, ApplicationDbContext>();   
    }
}
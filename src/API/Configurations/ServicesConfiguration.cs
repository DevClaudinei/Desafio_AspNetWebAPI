using AppServices.Services;
using DomainServices;
using DomainServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configurations;

public static class ServicesConfiguration
{
    public static void AddServicesConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();

        services.AddTransient<ICustomerAppService, CustomerAppService>();
    }
}
using DomainModels;
using DomainServices;
using DomainServices.Services;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppServices;
public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerService, CustomerService>();

        services.AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CustomerValidators>());

        services.AddTransient<ICustomerAppService, CustomerAppService>();

        return services;
    }
}
using AppServices.Services;
using DomainServices;
using DomainServices.GenericRepositories;
using DomainServices.GenericRepositories.Interface;
using DomainServices.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configurations;

public static class ServicesConfiguration
{
    public static void AddServicesConfiguration(this IServiceCollection services)
    {
        services.AddTransient<ICustomerService, CustomerService>();

        services.AddTransient<ICustomerAppService, CustomerAppService>();

        services.AddScoped<DbContext, ApplicationDbContext>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}
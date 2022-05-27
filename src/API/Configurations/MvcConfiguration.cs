using AppServices;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configurations;

public static class MvcConfiguration
{
    public static void AddMvcConfiguration(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<CustomerValidators>());
    }
}
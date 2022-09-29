using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configurations;

public static class MvcConfiguration
{
    public static void AddMvcConfiguration(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddFluentValidation(options => options
            .RegisterValidatorsFromAssembly(Assembly.Load("AppServices")));
    }
}
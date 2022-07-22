using EntityFrameworkCore.UnitOfWork.Extensions;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configurations;

public static class UnitOfWorkConfiguration
{
    public static void AddUnitOfWorkConfiguration(this IServiceCollection services)
    {
        services.AddUnitOfWork();
        services.AddUnitOfWork<ApplicationDbContext>();
    }
}

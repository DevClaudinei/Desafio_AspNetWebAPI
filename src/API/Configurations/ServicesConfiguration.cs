﻿using AppServices.Services;
using AppServices.Services.Interfaces;
using DomainServices;
using DomainServices.Services;
using DomainServices.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configurations;

public static class ServicesConfiguration
{
    public static void AddServicesConfiguration(this IServiceCollection services)
    {
        services.AddTransient<ICustomerService, CustomerService>();

        services.AddTransient<ICustomerAppService, CustomerAppService>();

        services.AddTransient<ICustomerBankInfoService, CustomerBankInfoService>();

        services.AddTransient<ICustomerBankInfoAppService, CustomerBankInfoAppService>();

        services.AddTransient<IProductService, ProductService>();

        services.AddTransient<IProductAppService, ProductAppService>();

        services.AddTransient<IPortfolioService, PortfolioService>();

        services.AddTransient<IPortfolioAppService, PortfolioAppService>();

        services.AddTransient<IOrderService, OrderService>();

        services.AddTransient<IOrderAppService, OrderAppService>();

        services.AddTransient<IPortfolioProductService, PortfolioProductService>();

        services.AddTransient<IPortfolioProductAppService, PortfolioProductAppService>();
    }
}
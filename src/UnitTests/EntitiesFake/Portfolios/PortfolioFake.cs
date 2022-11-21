using Bogus;
using DomainModels.Entities;
using System.Collections.Generic;
using UnitTests.EntitiesFake.Products;

namespace UnitTests.EntitiesFake.Portfolios;

public static class PortfolioFake
{
    public static Portfolio PortfolioFaker()
    {
        var productFake = ProductFake.ProductFakers(1);
        var portfolioFake = new Faker<Portfolio>("pt_BR")
            .CustomInstantiator(f => new Portfolio(
                    totalBalance: f.Random.Decimal(0, 0),
                    customerId: f.Random.Long(1, 1)
            )).Generate();
        portfolioFake.Id = 1L;
        portfolioFake.Products = (ICollection<Product>)ProductFake.ProductFakers(1);
        return portfolioFake;
    }

    public static IEnumerable<Portfolio> PortfolioFakers(int quantity)
    {
        var id = 1L;
        var portfolioFakers = new Faker<Portfolio>("pt_BR")
            .CustomInstantiator(f => new Portfolio(
                    totalBalance: f.Random.Decimal(0, 0),
                    customerId: f.Random.Long(1, 1)
            )).Generate(quantity);

        foreach (var portfolioFake in portfolioFakers)
        {
            portfolioFake.Id = id++;
        }

        return portfolioFakers;
    }
}
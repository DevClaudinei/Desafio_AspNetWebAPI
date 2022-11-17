using Bogus;
using DomainModels.Entities;
using System.Collections.Generic;
using UnitTests.EntitiesFake.Portfolios;
using UnitTests.EntitiesFake.Products;

namespace UnitTests.EntitiesFake;

public static class PortfolioProductFake
{
    public static PortfolioProduct PortfolioProductFaker()
    {
        var portfolioProductFake = new Faker<PortfolioProduct>("pt_BR")
            .CustomInstantiator(f => new PortfolioProduct(
                portfolioId: f.Random.Long(1, 1),
                productId: f.Random.Long(1, 1)
            )).Generate();

        portfolioProductFake.Id = 1;
        portfolioProductFake.Portfolio = PortfolioFake.PortfolioFaker();
        portfolioProductFake.Product = ProductFake.ProductFaker();

        return portfolioProductFake;
    }

    public static IEnumerable<PortfolioProduct> PortfolioProductFakers(int quantity)
    {
        var id = 1;
        var portfolioProductFakes = new Faker<PortfolioProduct>("pt_BR")
            .CustomInstantiator(f => new PortfolioProduct(
                portfolioId: f.Random.Long(1, 1),
                productId: f.Random.Long(1, 1)
            )).Generate(quantity);

        foreach (var portfolioProductFake in portfolioProductFakes)
        {
            portfolioProductFake.Id = id++;
        }

        return portfolioProductFakes;
    }
}
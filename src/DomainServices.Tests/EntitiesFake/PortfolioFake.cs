using Bogus;
using DomainModels.Entities;

namespace DomainServices.Tests.EntitiesFake;

public static class PortfolioFake
{
	public static Portfolio PortfolioFaker()
	{
		var productFake = ProductFake.ProductFakers(1);
        var portfolioFake = new Faker<Portfolio>("pt_BR")
			.CustomInstantiator(f => new Portfolio(
					f.Random.Decimal(0, 0),
					f.Random.Long(1, 1)
			)).Generate();
		portfolioFake.Id = 1L;
		portfolioFake.Products = ProductFake.ProductFakersToPortfolio();

		return portfolioFake;
	}

	public static IEnumerable<Portfolio> PortfolioFakers(int quantity)
	{
		var id = 1L;
		var portfolioFakers = new Faker<Portfolio>("pt_BR")
            .CustomInstantiator(f => new Portfolio(
                    f.Random.Decimal(0, 0),
                    f.Random.Long(1, 1)
            )).Generate(quantity);

		foreach (var portfolioFake in portfolioFakers)
		{
			portfolioFake.Id = id++;
        }

		return portfolioFakers;
    }
}
using Bogus;
using DomainModels.Entities;

namespace DomainServices.Tests.EntitiesFake;

public static class PortfolioProductFake
{
	public static PortfolioProduct PortfolioProductFaker()
	{
		var portfolioProductFake = new Faker<PortfolioProduct>("pt_BR")
			.CustomInstantiator(f => new PortfolioProduct(
					f.Random.Long(1,1),
					f.Random.Long(1,1)
			)).Generate();

		portfolioProductFake.Portfolio = PortfolioFake.PortfolioFaker();
		portfolioProductFake.Product = ProductFake.ProductFaker();

		return portfolioProductFake;
	}

	public static IEnumerable<PortfolioProduct> PortfolioProductFakers(int quantity)
	{
		var id = 1;
		var portfolioProductFakes = new Faker<PortfolioProduct>("pt_BR")
			.CustomInstantiator(f => new PortfolioProduct(
                f.Random.Long(1, 1),
                f.Random.Long(1, 1)
            )).Generate(quantity);

		foreach (var portfolioProductFake in portfolioProductFakes)
		{
			portfolioProductFake.Id = id++;
        }

		return portfolioProductFakes;
	}
}
using Bogus;
using DomainModels.Entities;

namespace DomainServices.Tests.EntitiesFake;

public static class ProductFake
{
	public static Product ProductFaker()
	{
		var productFake = new Faker<Product>("pt_BR")
			.CustomInstantiator(f => new Product(
					f.Random.Long(1, 1),
					f.Commerce.ProductName(),
					f.Random.Decimal(1)
				)).Generate();

		return productFake;
	}

	public static IEnumerable<Product> ProductFakers(int quantity)
	{
		var productFakes = new Faker<Product>("pt_BR")
            .CustomInstantiator(f => new Product(
                    f.Random.Long(1),
                    f.Commerce.ProductName(),
                    f.Random.Decimal(1)
                )).Generate(quantity);

		return productFakes;
    }

	public static ICollection<Product> ProductFakersToPortfolio()
	{
        var productFakes = new Faker<Product>("pt_BR")
			.CustomInstantiator(f => new Product(
				f.Random.Long(1),
				f.Commerce.ProductName(),
				f.Random.Decimal(1)
        )).Generate(1);

        return productFakes;
    }
}

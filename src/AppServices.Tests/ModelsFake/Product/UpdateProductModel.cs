using Application.Models.Product.Request;
using Bogus;

namespace AppServices.Tests.ModelsFake.Product;

public static class UpdateProductModel
{
	public static UpdateProductRequest ProductFake()
	{
		var updatePortfolioFake = new Faker<UpdateProductRequest>("pt_BR")
			.CustomInstantiator(f => new UpdateProductRequest(
				symbol: f.Commerce.ProductName(),
				unitPrice: f.Random.Decimal(1, 100)
			)).Generate();

		return updatePortfolioFake;
	}
}

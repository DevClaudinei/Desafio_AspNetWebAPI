using Application.Models.Product.Request;
using Bogus;

namespace AppServices.Tests.ModelsFake.Product;

public static class CreateProductModel
{
    public static CreateProductRequest ProductFake()
    {
        var createPortfolioFake = new Faker<CreateProductRequest>("pt_BR")
            .CustomInstantiator(f => new CreateProductRequest(
                symbol: f.Commerce.ProductName(),
                unitPrice: f.Random.Decimal(1, 100)
            )).Generate();

        createPortfolioFake.Id = 1;

        return createPortfolioFake;
    }
}

using Application.Models.Product.Request;
using Bogus;

namespace UnitTests.EntitiesFake.Products;

public static class CreateProductModel
{
    public static CreateProductRequest ProductFake()
    {
        var productFake = new Faker<CreateProductRequest>("pt_BR")
            .CustomInstantiator(f => new CreateProductRequest(
                symbol: f.Commerce.ProductName(),
                unitPrice: f.Random.Decimal(1, 100)
            )).Generate();

        productFake.Id = 1;

        return productFake;
    }
}
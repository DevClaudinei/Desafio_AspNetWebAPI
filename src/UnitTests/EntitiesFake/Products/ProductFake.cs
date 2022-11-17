using Bogus;
using DomainModels.Entities;
using System.Collections.Generic;

namespace UnitTests.EntitiesFake.Products;

public static class ProductFake
{
    public static Product ProductFaker()
    {
        var productFake = new Faker<Product>("pt_BR")
            .CustomInstantiator(f => new Product(
                id: f.Random.Long(1, 1),
                symbol: f.Commerce.ProductName(),
                unitPrice: f.Random.Decimal(1)
            )).Generate();

        return productFake;
    }

    public static IEnumerable<Product> ProductFakers(int quantity)
    {
        var productFakes = new Faker<Product>("pt_BR")
            .CustomInstantiator(f => new Product(
                id: f.Random.Long(1, 1),
                symbol: f.Commerce.ProductName(),
                unitPrice: f.Random.Decimal(1)
            )).Generate(quantity);

        return productFakes;
    }

    public static ICollection<Product> ProductFakersToPortfolio()
    {
        var productFakes = new Faker<Product>("pt_BR")
            .CustomInstantiator(f => new Product(
                id: f.Random.Long(1, 1),
                symbol: f.Commerce.ProductName(),
                unitPrice: f.Random.Decimal(1)
        )).Generate(1);

        return productFakes;
    }
}
﻿using Application.Models.Portfolio.Response;
using Application.Models.Product.Response;
using Bogus;

namespace AppServices.Tests.Portfolio;

public static class PortfolioResponseModel
{
	public static IEnumerable<PortfolioResult> PortfolioFake(int quantity)
	{
        var portfolioResult = new Faker<PortfolioResult>("pt_BR")
            .CustomInstantiator(f => new PortfolioResult(
                id: 1,
                totalBalance: 0,
                products: new Faker<ProductResult>("pt_BR")
                    .CustomInstantiator(x => new ProductResult(
                        id: 1,
                        symbol: x.Commerce.ProductName(),
                        unitPrice: x.Commerce.Random.Decimal(1)
                )).Generate(1),
                customerId: 1
            )).Generate(2);

		//var products = ProductResponseModel.ProductFake();
  //      portfolioResult.Products.Append(products);

		return portfolioResult;
	}

    //public static IEnumerable<PortfolioResult> PortfolioFakers(int quantity)
    //{
    //    var id = 1;
    //    var portfolioResult = new Faker<PortfolioResult>("pt_BR")
    //        .CustomInstantiator(f => new PortfolioResult(
    //            id: id++,
    //            totalBalance: 0,
    //            customerId: 1
    //        )).Generate(quantity);

    //    foreach (var portfolioFake in portfolioResult)
    //    {
    //        var products = ProductResponseModel.ProductFake();
    //        portfolioFake.Products.Append(products);
    //    }

    //    return portfolioResult;
    //}
}
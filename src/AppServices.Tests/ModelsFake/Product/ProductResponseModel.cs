﻿using Application.Models.Product.Response;
using Bogus;

namespace AppServices.Tests.ModelsFake.Product;

public static class ProductResponseModel
{
	public static ProductResult ProductFake()
	{
		var productResult = new Faker<ProductResult>("pt_BR")
			.CustomInstantiator(f => new ProductResult(
				id: 1,
				symbol: f.Commerce.ProductName(),
				unitPrice: f.Commerce.Random.Decimal(1)
			)).Generate();

		return productResult;
	}

    //public static IEnumerable<ProductResult> ProductFakers(int quantity)
    //{
    //    var id = 1;
    //    var productResult = new Faker<ProductResult>("pt_BR")
    //        .CustomInstantiator(f => new ProductResult(
    //            id: id++,
    //            symbol: f.Commerce.ProductName(),
    //            unitPrice: f.Commerce.Random.Decimal(1)
    //        )).Generate(quantity);

    //    return productResult;
    //}
}
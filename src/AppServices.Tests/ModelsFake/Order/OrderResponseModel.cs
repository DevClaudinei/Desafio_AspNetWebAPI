using Application.Models.Order;
using Bogus;

namespace AppServices.Tests.ModelsFake.Order;

public static class OrderResponseModel
{
	public static OrderResult OrderFake()
	{
		var orderResult = new Faker<OrderResult>("pt_BR")
			.CustomInstantiator(f => new OrderResult(
				id: f.Random.Long(1, 1),
				quotes: f.Random.Int(1),
				netValue: f.Random.Decimal(1, 1),
				convertedAt: f.Date.Recent()
				)).Generate();

		return orderResult;
	}
    
    public static IEnumerable<OrderResult> OrderFakers(int quantity)
    {
        var orderResult = new Faker<OrderResult>("pt_BR")
            .CustomInstantiator(f => new OrderResult(
                id: f.Random.Long(1, 1),
                quotes: f.Random.Int(1),
                netValue: f.Random.Decimal(1, 1),
                convertedAt: f.Date.Recent()
                )).Generate(quantity);

        return orderResult;
    }
}
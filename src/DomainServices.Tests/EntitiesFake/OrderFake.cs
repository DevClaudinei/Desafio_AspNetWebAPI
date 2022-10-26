using Bogus;
using DomainModels.Entities;

namespace DomainServices.Tests.EntitiesFake;

public static class OrderFake
{
	public static Order OrderFaker()
	{
		var orderFake = new Faker<Order>("pt_BR")
			.CustomInstantiator(f => new Order(
				f.Random.Int(1),
                f.Random.Int(1)
            )).Generate();

		orderFake.Id = 1;
		orderFake.NetValue = 1;
		orderFake.Quotes = 1;

		return orderFake;
	}

	public static IEnumerable<Order> OrderFakers(int quantity)
	{
        var id = 1;
        var orderFakes = new Faker<Order>("pt_BR")
            .CustomInstantiator(f => new Order(
                f.Random.Int(1),
                f.Random.Int(1)
            )).Generate(quantity);

		foreach (var orderfake in orderFakes)
		{
			orderfake.Id = id++;
            orderfake.NetValue = 1;
            orderfake.Quotes = 1;
            orderfake.Direction = DomainModels.Enum.OrderDirection.Buy;
        }

		return orderFakes;
    }
}

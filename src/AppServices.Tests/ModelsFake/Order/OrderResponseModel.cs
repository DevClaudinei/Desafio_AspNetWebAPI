using Application.Models.Order;
using Bogus;

namespace AppServices.Tests.ModelsFake.Order;

public static class OrderResponseModel
{
    public static IEnumerable<OrderResult> OrderFakers(int quantity)
    {
        var id = 1L;
        var orderResult = new Faker<OrderResult>("pt_BR")
            .CustomInstantiator(f => new OrderResult(
                id: f.Random.Long(0, 0),
                quotes: f.Random.Int(1),
                netValue: f.Random.Decimal(1, 1),
                convertedAt: f.Date.Recent(),
                direction: Application.Models.Enum.OrderDirection.Buy
                )).Generate(quantity);

        foreach (var order in orderResult)
        {
            order.Id += id;
            if (order.Id % 2 != 0)
                order.Direction = Application.Models.Enum.OrderDirection.Sell;
        }

        return orderResult;
    }
}
﻿using Application.Models.Enum;
using Application.Models.Order;
using Bogus;
using System.Collections.Generic;

namespace UnitTests.EntitiesFake.Orders;

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
                direction: OrderDirection.Buy
                )).Generate(quantity);

        foreach (var order in orderResult)
        {
            order.Id += id;
            if (order.Id % 2 != 0)
                order.Direction = OrderDirection.Sell;
        }

        return orderResult;
    }
}
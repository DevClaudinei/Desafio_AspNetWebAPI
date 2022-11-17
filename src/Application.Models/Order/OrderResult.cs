using Application.Models.Enum;
using System;

namespace Application.Models.Order;

public class OrderResult
{
    public OrderResult(long id, int quotes, decimal netValue, DateTime convertedAt, OrderDirection direction)
    {
        Id = id;
        Quotes = quotes;
        NetValue = netValue;
        ConvertedAt = convertedAt;
        Direction = direction;
    }

    public long Id { get; init; }
    public int Quotes { get; set; }
    public decimal NetValue { get; set; }
    public DateTime ConvertedAt { get; set; }
    public OrderDirection Direction { get; set; }
}

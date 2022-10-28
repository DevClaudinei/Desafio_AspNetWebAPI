using System;

namespace Application.Models.Order;

public class OrderResult
{
    public OrderResult(long id, int quotes, decimal netValue, DateTime convertedAt)
    {
        Id = id;
        Quotes = quotes;
        NetValue = netValue;
        ConvertedAt = convertedAt;
    }

    public long Id { get; set; }
    public int Quotes { get; set; }
    public decimal NetValue { get; set; }
    public DateTime ConvertedAt { get; set; }
}

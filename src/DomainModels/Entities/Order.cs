using System;

namespace DomainModels.Entities;
public class Order
{
    protected Order() { }

    public Order(decimal unitPrice, int quotes)
    {
        NetValue = unitPrice * quotes;
    }

    public Order(int quotes, decimal unitPrice, DateTime convertedAt)
    {
        Quotes = quotes;
        NetValue = unitPrice * quotes;
        ConvertedAt = convertedAt;
    }

    public long Id { get; set; }
    public int Quotes { get; set; }
    public decimal NetValue { get; set; }
    public DateTime ConvertedAt { get; set; }
    public long PortfolioId { get; set; }
    public virtual Portfolio Portfolio { get; set; }
    public long ProductId { get; set; }
    public virtual Product Product { get; set; }
}

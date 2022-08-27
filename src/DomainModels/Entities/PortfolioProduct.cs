using System;

namespace DomainModels.Entities;

public class PortfolioProduct
{
    protected PortfolioProduct() { }

    public PortfolioProduct(int quotes, decimal netValue, DateTime convertedAt)
    {
        Quotes = quotes;
        NetValue = netValue;
        ConvertedAt = convertedAt;
    }

    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public virtual Portfolio Portfolio { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int Quotes { get; set; }
    public decimal NetValue { get; set; }
    public DateTime ConvertedAt { get; set; }
}

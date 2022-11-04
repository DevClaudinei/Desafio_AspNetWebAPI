using DomainModels.Enum;
using System;

namespace DomainModels.Entities;
public class Order
{
    protected Order() { }

    public Order(long portfolioId, long productId)
    {
        PortfolioId = portfolioId;
        ProductId = productId;
    }

    public long Id { get; set; }
    public int Quotes { get; set; }
    public decimal NetValue { get; set; }
    public DateTime ConvertedAt { get; set; }
    public OrderDirection Direction { get; set; }
    public long PortfolioId { get; set; }
    public virtual Portfolio Portfolio { get; set; }
    public long ProductId { get; set; }
    public virtual Product Product { get; set; }
}
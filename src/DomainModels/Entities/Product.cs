using System.Collections.Generic;

namespace DomainModels.Entities;

public class Product
{
    protected Product() { }

    public Product(long id, string symbol, decimal unitPrice)
    {
        Id = id;
        Symbol = symbol;
        UnitPrice = unitPrice;
    }

    public long Id { get; set; }
    public string Symbol { get; set; }
    public decimal UnitPrice { get; set; }
    public virtual ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
}
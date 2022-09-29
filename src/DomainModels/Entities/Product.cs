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
    public string Symbol { get; set; } // nome do ativo
    public decimal UnitPrice { get; set; } // preço de cada cota de um ativo
    public virtual ICollection<Portfolio> Portfolios { get; set; }
}
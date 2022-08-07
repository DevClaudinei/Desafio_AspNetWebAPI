using System;

namespace DomainModels.Entities;

public class Product
{
    protected Product() { }

    public Product(
        string symbol,
        int quotes,
        decimal unitPrice,
        decimal netValue,
        DateTime convertedAt,
        Guid productId
    )
    {
        Symbol = symbol;
        Quotes = quotes;
        UnitPrice = unitPrice;
        NetValue = netValue;
        ConvertedAt = convertedAt;
        ProductId = productId;
    }

    public Guid ProductId { get; set; }
    public string Symbol { get; set; } // nome do ativo
    public int Quotes { get; set; } // quantidade de cotas
    public decimal UnitPrice { get; set; } // preço de cada cota de um ativo
    public decimal NetValue { get; set; } // valor liquido total multiplicando Quotes pelo UnitPrice
    public DateTime ConvertedAt { get; set; } // data da compra
    public Portfolio Portfolio { get; set; }
    public Guid PortfolioId { get; set; }
}

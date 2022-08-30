using System;

namespace Application.Models.Product.Request;

public class CreateProductRequest
{
    public CreateProductRequest(
        string symbol,
        decimal unitPrice
    )
    {
        Symbol = symbol;
        UnitPrice = unitPrice;
    }
    public long Id { get; set; }
    public string Symbol { get; set; } // nome do ativo
    public int Quotes { get; set; } // quantidade de cotas
    public decimal UnitPrice { get; set; } // preço de cada cota de um ativo
    public decimal NetValue { get; set; } // valor liquido total multiplicando Quotes pelo UnitPrice
    public DateTime ConvertedAt { get; set; } // data da compra
    public long PortfolioId { get; set; }
}

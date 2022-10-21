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
    public string Symbol { get; set; }
    public int Quotes { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal NetValue { get; set; }
    public DateTime ConvertedAt { get; set; }
    public long PortfolioId { get; set; }
}
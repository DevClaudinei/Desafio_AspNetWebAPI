using System;

namespace Application.Models.Product.Request;

public class CreateProductRequest
{
    public CreateProductRequest(string symbol, decimal unitPrice)
    {
        Symbol = symbol;
        UnitPrice = unitPrice;
    }

    public long Id { get; set; }
    public string Symbol { get; set; }
    public decimal UnitPrice { get; set; }
}
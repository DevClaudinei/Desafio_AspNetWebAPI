using System;

namespace Application.Models.Product.Response;

public class ProductResult
{
    public Guid ProductId { get; init; }
    public string Symbol { get; init; } // nome do ativo
    public DateTime ConvertedAt { get; init; } // data da compra
}

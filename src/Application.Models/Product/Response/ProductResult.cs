using System;
using System.Collections.Generic;

namespace Application.Models.Product.Response;

public class ProductResult
{
    public Guid Id { get; init; }
    public string Symbol { get; init; } // nome do ativo
    public decimal NetValue { get; init; }
    public DateTime ConvertedAt { get; init; } // data da compra
}

using System;
using System.Collections.Generic;

namespace Application.Models.Product.Response;

public class ProductResult
{
    public long Id { get; init; }
    public string Symbol { get; init; } // nome do ativo
    public decimal UnitPrice { get; set; } // preço de cada cota de um ativo
}

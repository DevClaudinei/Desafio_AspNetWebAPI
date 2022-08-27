using Application.Models.PortfolioProduct;
using System.Collections.Generic;

namespace Application.Models.Portfolio.Response;

public class PortfolioResult
{
    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public IEnumerable<PortfolioProductResult> Products { get; set; } // lista de produtos comprados
}

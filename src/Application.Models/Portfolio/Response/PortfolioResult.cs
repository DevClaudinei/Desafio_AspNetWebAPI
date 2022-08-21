using Application.Models.Product.Response;
using System.Collections.Generic;

namespace Application.Models.Portfolio.Response;

public class PortfolioResult
{
    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public virtual ICollection<ProductResult> Products { get; set; } // lista de produtos comprados
}

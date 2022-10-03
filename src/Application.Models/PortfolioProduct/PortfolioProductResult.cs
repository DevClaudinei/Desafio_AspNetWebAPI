using Application.Models.Portfolio.Response;
using Application.Models.Product.Response;
using System.Collections.Generic;

namespace Application.Models.PortfolioProduct;

public class PortfolioProductResult
{
    public long Id { get; set; }
    public long PortfolioId { get; set; }
    public long ProductId { get; set; }
    public IEnumerable<PortfolioResult> Portfolios { get; set; }
    public IEnumerable<ProductResult> Products { get; set; }
}

using Application.Models.Product.Response;
using System.Collections.Generic;

namespace Application.Models.Portfolio.Response;

public class PortfolioResult
{
    public virtual ICollection<ProductResult> Products { get; set; } // lista de produtos comprados
}

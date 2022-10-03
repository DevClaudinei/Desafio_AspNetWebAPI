using Application.Models.Order;
using System.Collections.Generic;

namespace Application.Models.PortfolioProduct;

public class PortfolioProductResult
{
    public long Id { get; set; }
    public long PortfolioId { get; set; }
    public long ProductId { get; set; }
    public virtual ICollection<OrderResult> Orders { get; set; }
}

using System;

namespace DomainModels.Entities;

public class PortfolioProduct
{
    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public virtual Portfolio Portfolio { get; set; }
    public Guid ProductId { get; set; }
    public virtual Product Product { get; set; }
}

using System;

namespace Application.Models.PortfolioProduct;

public class PortfolioProductResult
{
    public Guid Id { get; set; }
    public decimal NetValue { get; set; }
    public DateTime ConvertedAt { get; set; }
}

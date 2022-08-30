using System;

namespace Application.Models.PortfolioProduct.Response;

public class PortfolioProductResult
{
    public long Id { get; set; }
    public int Quotes { get; set; }
    public decimal NetValue { get; set; }
    public DateTime ConvertedAt { get; set; }
}

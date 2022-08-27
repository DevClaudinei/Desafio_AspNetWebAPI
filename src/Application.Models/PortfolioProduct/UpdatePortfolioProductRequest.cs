using System;

namespace Application.Models.PortfolioProduct;

public class UpdatePortfolioProductRequest
{
    public UpdatePortfolioProductRequest(int quotes, decimal netValue)
    {
        Quotes = quotes;
        NetValue = netValue;
    }

    public Guid PortfolioId { get; set; }
    public Guid ProductId { get; set; }
    public int Quotes { get; set; }
    public decimal NetValue { get; set; }

}

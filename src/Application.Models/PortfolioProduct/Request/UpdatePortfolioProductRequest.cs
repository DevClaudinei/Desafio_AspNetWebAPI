namespace Application.Models.PortfolioProduct.Request;

public class UpdatePortfolioProductRequest
{
    public UpdatePortfolioProductRequest(int quotes, decimal netValue)
    {
        Quotes = quotes;
        NetValue = netValue;
    }

    public long PortfolioId { get; set; }
    public long ProductId { get; set; }
    public int Quotes { get; set; }
    public decimal NetValue { get; set; }
}
namespace Application.Models.Product.Request;

public class UpdateProductRequest
{
    public UpdateProductRequest(
        string symbol,
        decimal unitPrice
    )
    {
        Symbol = symbol;
        UnitPrice = unitPrice;
    }

    public long Id { get; set; }
    public string Symbol { get; set; }
    public decimal UnitPrice { get; set; }
    public long PortfolioId { get; set; }
    public int Quotes { get; set; }
}
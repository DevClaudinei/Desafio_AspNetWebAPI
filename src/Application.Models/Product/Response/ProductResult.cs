namespace Application.Models.Product.Response;

public class ProductResult
{
    public ProductResult(long id, string symbol, decimal unitPrice)
    {
        Id = id;
        Symbol = symbol;
        UnitPrice = unitPrice;
    }

    public long Id { get; init; }
    public string Symbol { get; init; }
    public decimal UnitPrice { get; set; }
}
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
    public string Symbol { get; set; } // nome do ativo
    public decimal UnitPrice { get; set; } // preço de cada cota de um ativo
    public long PortfolioId { get; set; }
    public int Quotes { get; set; } // quantidade de cotas
}
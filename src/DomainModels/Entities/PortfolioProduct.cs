namespace DomainModels.Entities;

public class PortfolioProduct
{
    protected PortfolioProduct() { }

    public long Id { get; set; }
    public long PortfolioId { get; set; }
    public virtual Portfolio Portfolio { get; set; }
    public long ProductId { get; set; }
    public virtual Product Product { get; set; }
}
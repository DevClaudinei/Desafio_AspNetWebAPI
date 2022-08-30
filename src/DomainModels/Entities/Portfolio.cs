using System.Collections.Generic;

namespace DomainModels.Entities;

public class Portfolio : BaseModel
{
    protected Portfolio() { }

    public Portfolio(decimal totalBalance, long customerId)
    {
        TotalBalance = totalBalance;
        CustomerId = customerId;
    }

    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual ICollection<PortfolioProduct> PortfolioProducts { get; set; }
}

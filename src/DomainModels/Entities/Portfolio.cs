using System;
using System.Collections.Generic;

namespace DomainModels.Entities;

public class Portfolio : IEntity
{
    public Portfolio() { }

    public Portfolio(decimal totalBalance, long customerId)
    {
        TotalBalance = totalBalance;
        CustomerId = customerId;
    }

    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<PortfolioProduct> PortfolioProducts { get; set; } = new List<PortfolioProduct>();
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
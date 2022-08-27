using System;
using System.Collections.Generic;

namespace DomainModels.Entities;

public class Portfolio : BaseModel
{
    protected Portfolio() { }

    public Portfolio(decimal totalBalance, Guid customerId)
    {
        TotalBalance = totalBalance;
        CustomerId = customerId;
    }

    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    //public ICollection<Product> Products { get; set; } //= new List<Product>(); // lista de produtos comprados
    public Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual ICollection<PortfolioProduct> PortfolioProducts { get; set; }
}

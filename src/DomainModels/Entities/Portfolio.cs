using System;
using System.Collections.Generic;

namespace DomainModels.Entities;

public class Portfolio : BaseModel
{
    protected Portfolio() { }

    public Portfolio(decimal totalBalance, ICollection<PortfolioProduct> portfoliosProducts, Guid customerId)
    {
        TotalBalance = totalBalance;
        PortfoliosProducts = portfoliosProducts;
        CustomerId = customerId;
    }

    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public ICollection<PortfolioProduct> PortfoliosProducts { get; set; } // lista de produtos comprados
    public Guid CustomerId { get; set; }
}

using System;
using System.Collections.Generic;

namespace DomainModels.Entities;

public class Portfolio : BaseModel
{
    protected Portfolio() { }

    public Portfolio(decimal totalBalance, ICollection<Product> products, Guid customerId)
    {
        TotalBalance = totalBalance;
        Products = products;
        CustomerId = customerId;
    }

    public decimal TotalBalance { get; set; } // patrimônio da carteira somando todos os ativos
    public virtual ICollection<Product> Products { get; set; } // lista de produtos comprados
    public Guid CustomerId { get; set; }
}

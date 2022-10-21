using System;
using System.Collections.Generic;

namespace DomainModels.Entities;

public class Portfolio : IEntity
{
    protected Portfolio() { }

    public Portfolio(decimal totalBalance, long customerId)
    {
        TotalBalance = totalBalance;
        CustomerId = customerId;
    }

    public decimal TotalBalance { get; set; }
    public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual ICollection<Product> Products { get; set; }
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
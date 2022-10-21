using System;

namespace DomainModels.Entities;

public class CustomerBankInfo : IEntity
{
    protected CustomerBankInfo() { }

    public CustomerBankInfo(long customerId) 
    {
        CustomerId = customerId;
        Account = Guid.NewGuid().ToString();
    }

    public string Account { get; set; }
    public decimal AccountBalance { get; set; } = 0;
    public virtual Customer Customer { get; set; }
    public long CustomerId { get; set; }
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
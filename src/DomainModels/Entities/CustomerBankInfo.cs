using System;

namespace DomainModels.Entities;

public class CustomerBankInfo : IEntity
{
    public CustomerBankInfo(long customerId) 
    {
        CustomerId = customerId;
        Account = new Random().Next(1, 100).ToString();
    }

    public string Account { get; set; } // código da conta
    public decimal AccountBalance { get; set; } = 0; // saldo da conta
    public virtual Customer Customer { get; set; }
    public long CustomerId { get; set; }
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
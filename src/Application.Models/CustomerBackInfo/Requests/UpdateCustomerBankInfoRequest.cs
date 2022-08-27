using System;

namespace Application.Models;

public class UpdateCustomerBankInfoRequest
{
    protected UpdateCustomerBankInfoRequest() { }

    public UpdateCustomerBankInfoRequest(decimal accountBalance)
    { 
        AccountBalance = accountBalance;
    }

    public Guid Id { get; set; }
    public string Account { get; set; }
    public decimal AccountBalance { get; set; }
    public Guid CustomerId { get; set; }
}
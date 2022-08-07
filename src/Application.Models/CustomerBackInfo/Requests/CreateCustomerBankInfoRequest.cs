using Application.Models.Response;
using System;

namespace Application.Models;
public class CreateCustomerBankInfoRequest
{
    public CreateCustomerBankInfoRequest(string account, decimal accountBalance, Guid customerId)
    {
        Account = account;
        AccountBalance = accountBalance;
        CustomerId = customerId;
    }
    public string Account { get; set; }
    public decimal AccountBalance { get; set; }
    public Guid CustomerId { get; set; }
}
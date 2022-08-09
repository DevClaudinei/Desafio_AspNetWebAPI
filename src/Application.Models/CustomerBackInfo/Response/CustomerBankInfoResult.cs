using System;

namespace Application.Models.Response;

public class CustomerBankInfoResult
{
    public Guid Id { get; init; }
    public string Account { get; init; }
    public decimal AccountBalance { get; init; }
    public Guid CustomerId { get; init; }
}
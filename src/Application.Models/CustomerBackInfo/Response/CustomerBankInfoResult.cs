using System;

namespace Application.Models.Response;

public class CustomerBankInfoResult
{
    public Guid Id { get; init; }
    public string Account { get; set; }
    public decimal AccountBalance { get; set; }
}
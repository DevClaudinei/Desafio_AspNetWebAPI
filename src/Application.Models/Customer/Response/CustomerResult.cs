using Application.Models.Portfolio.Response;
using Application.Models.Response;
using System;
using System.Collections.Generic;

namespace Application.Models;

public class CustomerResult
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Cpf { get; init; }
    public string Country { get; init; }
    public string City { get; init; }
    public CustomerBankInfoResult CustomerBankInfo { get; init; } 
    public ICollection<PortfolioResult> Portfolios { get; init; }
}
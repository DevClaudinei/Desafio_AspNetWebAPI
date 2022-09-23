using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioAppService
{
    long Create(CreatePortfolioRequest portfolioCreate);
    IEnumerable<PortfolioResult> GetAll();
    PortfolioResult GetPortfolioById(long id);
    decimal GetTotalBalance(long portfolioId);
    public bool UpdateBalanceAfterPurchase(PortfolioResult portfolioResult, decimal purchaseValue);
    void Delete(long id);
    long Invest(CreateInvestmentRequest request, long customerId);
}
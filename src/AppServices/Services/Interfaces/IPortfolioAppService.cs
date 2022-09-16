using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioAppService
{
    long CreatePortfolio(CreatePortfolioRequest portfolioCreate);
    IEnumerable<PortfolioResult> GetAllPortfolios();
    PortfolioResult GetPortfolioById(long id);
    decimal GetTotalBalance(long portfolioId);
    public bool UpdateBalanceAfterPurchase(PortfolioResult portfolioResult, decimal purchaseValue);
    void Delete(long id);
}
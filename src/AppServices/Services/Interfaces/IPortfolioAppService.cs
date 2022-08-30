using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioAppService
{
    (bool isValid, string message) CreatePortfolio(CreatePortfolioRequest portfolioCreate);
    IEnumerable<PortfolioResult> GetAllPortfolios();
    PortfolioResult GetPortfolioById(long id);
    (bool isValid, string message) GetTotalBalance(long portfolioId);
    public bool UpdateBalanceAfterPurchase(PortfolioResult portfolioResult, decimal purchaseValue);
    bool Delete(long id);
}

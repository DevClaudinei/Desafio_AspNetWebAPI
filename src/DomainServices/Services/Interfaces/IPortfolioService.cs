using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioService
{
    long CreatePortfolio(Portfolio portfolio);
    IEnumerable<Portfolio> GetAllPortfolios();
    Portfolio GetPortfolioById(long id);
    decimal GetTotalBalance(long portfolioId);
    void Update(Portfolio portfolio);
    public bool UpdateBalanceAfterPurchase(Portfolio portfolio);
    void Delete(long id);
}
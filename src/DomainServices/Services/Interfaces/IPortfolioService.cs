using DomainModels.Entities;
using System;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioService
{
    (bool isValid, string message) CreatePortfolio(Portfolio portfolio);
    IEnumerable<Portfolio> GetAllPortfolios();
    Portfolio GetPortfolioById(long id);
    (bool isValid, string message) GetTotalBalance(long portfolioId);
    (bool isValid, string message) Update(Portfolio portfolio);
    public bool UpdateBalanceAfterPurchase(Portfolio portfolio);
    bool Delete(long id);
}

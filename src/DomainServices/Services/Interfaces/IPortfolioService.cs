using DomainModels.Entities;
using System;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioService
{
    (bool isValid, string message) CreatePortfolio(Portfolio portfolio);
    IEnumerable<Portfolio> GetAllPortfolios();
    Portfolio GetPortfolioById(Guid id);
    (bool isValid, string message) GetTotalBalance(Guid portfolioId);
    (bool isValid, string message) Update(Portfolio portfolio);
    bool Delete(Guid id);
}

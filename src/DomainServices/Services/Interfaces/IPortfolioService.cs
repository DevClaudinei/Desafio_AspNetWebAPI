using DomainModels.Entities;
using System;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioService
{
    (bool isValid, string message) CreatePortfolio(Portfolio portfolio);
    IEnumerable<Portfolio> GetAllPortfolios();
    Portfolio PortfolioById(Guid id);
    decimal GetTotalBalance(Guid portfolioId);
    bool Delete(Guid id);
}

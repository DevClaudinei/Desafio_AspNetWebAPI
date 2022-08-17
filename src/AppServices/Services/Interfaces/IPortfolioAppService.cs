using Application.Models;
using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using DomainModels.Entities;
using System;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioAppService
{
    (bool isValid, string message) CreatePortfolio(PortfolioCreate portfolioCreate);
    IEnumerable<PortfolioResult> GetAllPortfolios();
    PortfolioResult PortfolioById(Guid id);
    decimal GetTotalBalance(Guid portfolioId);
    bool Delete(Guid id);
}

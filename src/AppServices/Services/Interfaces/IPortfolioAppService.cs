using Application.Models;
using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using DomainModels.Entities;
using System;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioAppService
{
    (bool isValid, string message) CreatePortfolio(CreatePortfolioRequest portfolioCreate);
    IEnumerable<PortfolioResult> GetAllPortfolios();
    PortfolioResult GetPortfolioById(Guid id);
    (bool isValid, string message) GetTotalBalance(Guid portfolioId);
    (bool isValid, string message) Update(UpdatePortfolioRequest updatePortfolioRequest);
    bool Delete(Guid id);
}

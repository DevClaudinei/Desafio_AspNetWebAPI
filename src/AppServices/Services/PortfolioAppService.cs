using Application.Models;
using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using Application.Models.Product.Request;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class PortfolioAppService : IPortfolioAppService
{
    private readonly IPortfolioService _customerService;
    private readonly IMapper _mapper;

    public PortfolioAppService(IPortfolioService customerService, IMapper mapper)
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public (bool isValid, string message) CreatePortfolio(PortfolioCreate portfolioCreate)
    {
        var portfolio = _mapper.Map<Portfolio>(portfolioCreate);
        var createdPortfolio = _customerService.CreatePortfolio(portfolio);

        if (createdPortfolio.isValid) return (true, createdPortfolio.message);

        return (false, createdPortfolio.message);
    }

    public IEnumerable<PortfolioResult> GetAllPortfolios()
    {
        var portfoliosFound = _customerService.GetAllPortfolios();
        return _mapper.Map<IEnumerable<PortfolioResult>>(portfoliosFound);
    }

    public PortfolioResult PortfolioById(Guid id)
    {
        var portfolioFound = _customerService.PortfolioById(id);
        return _mapper.Map<PortfolioResult>(portfolioFound);
    }

    public decimal GetTotalBalance(Guid portfolioId)
    {
        return _customerService.GetTotalBalance(portfolioId);
    }

    public bool Delete(Guid id)
    {
        var deletedPortfolio = _customerService.Delete(id);
        return deletedPortfolio;
    }
}

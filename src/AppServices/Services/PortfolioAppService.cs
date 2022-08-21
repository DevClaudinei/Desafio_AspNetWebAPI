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
    private readonly IMapper _mapper;
    private readonly IPortfolioService _customerService;
    private readonly IProductAppService _productAppService;

    public PortfolioAppService(IPortfolioService customerService, IMapper mapper, IProductAppService productAppService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
    }

    public (bool isValid, string message) CreatePortfolio(CreatePortfolioRequest portfolioCreate)
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

    public PortfolioResult GetPortfolioById(Guid id)
    {
        var portfolioFound = _customerService.GetPortfolioById(id);
        return _mapper.Map<PortfolioResult>(portfolioFound);
    }

    public (bool isValid, string message) GetTotalBalance(Guid portfolioId)
    {
        return _customerService.GetTotalBalance(portfolioId);
    }

    public (bool isValid, string message) Update(UpdatePortfolioRequest updatePortfolioRequest)
    {

        foreach (var item in updatePortfolioRequest.Products)
        {
            var idProductToInsertInPortfolio = item.Id;
            var x = _productAppService.GetProductById(idProductToInsertInPortfolio);

            if (x == null) return (false, $"Product para o ID: {idProductToInsertInPortfolio} não localizado.");

            updatePortfolioRequest.Products.Add(item);
            break;
        }

        var updatedPortfolio = _mapper.Map<Portfolio>(updatePortfolioRequest);
        return _customerService.Update(updatedPortfolio);
    }

    public bool Delete(Guid id)
    {
        var deletedPortfolio = _customerService.Delete(id);
        return deletedPortfolio;
    }
}

using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using Application.Models.PortfolioProduct;
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
    private readonly IPortfolioService _portfolioService;
    private readonly IProductAppService _productAppService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

    public PortfolioAppService(
        IPortfolioService portfolioService,
        IMapper mapper,
        IProductAppService productAppService,
        ICustomerBankInfoAppService customerBankInfoAppService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
    }

    public (bool isValid, string message) CreatePortfolio(CreatePortfolioRequest portfolioCreate)
    {
        var portfolio = _mapper.Map<Portfolio>(portfolioCreate);
        var hasCustomerBankInfo = _customerBankInfoAppService.GetAllCustomerBankInfo();
        
        foreach (var customerBankInfo in hasCustomerBankInfo)
        {
            if (customerBankInfo.CustomerId != portfolio.CustomerId) 
                return (false, $"Não é possível criar portfolio para o Customer com Id: {portfolio.CustomerId}");
        }

        var createdPortfolio = _portfolioService.CreatePortfolio(portfolio);

        if (createdPortfolio.isValid) return (true, createdPortfolio.message);

        return (false, createdPortfolio.message);

    }

    public IEnumerable<PortfolioResult> GetAllPortfolios()
    {
        var portfoliosFound = _portfolioService.GetAllPortfolios();

        foreach (var portfolio in portfoliosFound)
        {   
            foreach (var item in portfolio.PortfolioProducts)
            {
                var x = _portfolioService.GetPortfolioById(item.ProductId);
            }
        }
       
        return _mapper.Map<IEnumerable<PortfolioResult>>(portfoliosFound);
    }

    public PortfolioResult GetPortfolioById(Guid id)
    {
        
        var portfolioFound = _portfolioService.GetPortfolioById(id);

        if (portfolioFound is null) return null;

        var portfolioMapp = _mapper.Map<PortfolioResult>(portfolioFound);
        portfolioMapp.Products = _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolioFound.PortfolioProducts);

        return portfolioMapp;
    }

    public (bool isValid, string message) GetTotalBalance(Guid portfolioId)
    {
        return _portfolioService.GetTotalBalance(portfolioId);
    }

    public (bool isValid, string message) AddProduct(UpdatePortfolioProductRequest updatePortfolioProductRequest)
    {
        var portfolio = _portfolioService.GetPortfolioById(updatePortfolioProductRequest.PortfolioId);
        var product = _productAppService.GetProductById(updatePortfolioProductRequest.ProductId);

        if (portfolio is null || product is null) 
            return (false, $"Não é possível incluir o produto: {product} no portfolio: {portfolio}");

        var portfolioProduct = _mapper.Map<PortfolioResult>(portfolio);
        
        foreach (var item in portfolioProduct.Products)
        {
            item.NetValue = updatePortfolioProductRequest.Quotes * product.UnitPrice;
            portfolio.TotalBalance += item.NetValue;
            portfolio.PortfolioProducts.Add(_mapper.Map<PortfolioProduct>(item));
        }


        portfolio.PortfolioProducts.Clear();
        
        return _portfolioService.Update(portfolio);
    }

    public bool Delete(Guid id)
    {
        var deletedPortfolio = _portfolioService.Delete(id);
        return deletedPortfolio;
    }
}
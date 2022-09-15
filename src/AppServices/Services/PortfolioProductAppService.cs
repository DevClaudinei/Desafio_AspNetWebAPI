using Application.Models.Portfolio.Response;
using Application.Models.PortfolioProduct.Request;
using Application.Models.PortfolioProduct.Response;
using Application.Models.Product.Response;
using Application.Models.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class PortfolioProductAppService : IPortfolioProductAppService
{
    private readonly IMapper _mapper;
    private readonly IPortfolioProductService _portfolioProductService;
    private readonly IPortfolioAppService _portfolioAppService;
    private readonly IProductAppService _productAppService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

    public PortfolioProductAppService(
        IMapper mapper,
        IPortfolioProductService portfolioProductService,
        IPortfolioAppService portfolioAppService,
        IProductAppService productAppService,
        ICustomerBankInfoAppService customerBankInfoAppService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));
        _portfolioAppService = portfolioAppService ?? throw new ArgumentNullException(nameof(portfolioAppService));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
    }

    public IEnumerable<PortfolioProductResult> GetAllPortfolioProduct()
    {
        var portfolioProductsFound = _portfolioProductService.GetAllPortfolioProduct();
        return _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolioProductsFound);
    }

    public PortfolioProductResult GetPortfolioProductById(long id)
    {
        var portfolioProductFound = _portfolioProductService.GetPortfolioProductById(id);
        if (portfolioProductFound is null) return null;

        return _mapper.Map<PortfolioProductResult>(portfolioProductFound);
    }

    private ProductResult ProductExists(long productId)
    {
        var productFound = _productAppService.GetProductById(productId);

        if (productFound is null)
            throw new CustomerException($"Product not found to the Id: {productId}");

        return productFound;
    }

    private PortfolioResult PortfolioExists(long portfolioId)
    {
        var portfolioFound = _portfolioAppService.GetPortfolioById(portfolioId);

        if (portfolioFound is null)
            throw new CustomerException($"Portfolio not found to the Id: {portfolioId}");

        return portfolioFound;
    }

    public long Invest(InvestmentRequest request, long customerBankId)
    {
        var productFound = ProductExists(request.ProductId);
        var portfolioFound = PortfolioExists(request.PortfolioId);
        var customerBankInfoFound = _customerBankInfoAppService.GetCustomerBankInfoById(customerBankId);
        var investment = _mapper.Map<PortfolioProduct>(request);
        
        CheckCustomerAccountBalance(investment, customerBankInfoFound.AccountBalance, productFound);
        
        var createdInvestment = _portfolioProductService.Add(investment);

        PostInvestmentUpdates(portfolioFound, customerBankInfoFound, investment);

        return createdInvestment;
    }

    private void PostInvestmentUpdates(PortfolioResult portfolioFound, CustomerBankInfoResult customerBankInfoFound, PortfolioProduct investment)
    {
        portfolioFound.TotalBalance += investment.NetValue;
        _portfolioAppService.UpdateBalanceAfterPurchase(portfolioFound, investment.NetValue);
        _customerBankInfoAppService.UpdateBalanceAfterPurchase(customerBankInfoFound, investment.NetValue);
    }

    private bool CheckCustomerAccountBalance(PortfolioProduct investment, decimal accountBalance, ProductResult productFound)
    {
        investment.NetValue = productFound.UnitPrice * investment.Quotes;

        if (accountBalance < investment.NetValue)
            throw new CustomerException($"Insufficient balance to purchase the product with the Id: {productFound.Id}.");

        return true;
    }
}
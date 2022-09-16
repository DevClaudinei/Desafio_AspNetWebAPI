using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
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

public class PortfolioAppService : IPortfolioAppService
{
    private readonly IMapper _mapper;
    private readonly IPortfolioService _portfolioService;
    private readonly IProductAppService _productAppService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

    public PortfolioAppService(
        IMapper mapper,
        IPortfolioService portfolioService,
        IProductAppService productAppService,
        ICustomerBankInfoAppService customerBankInfoAppService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
    }

    public long CreatePortfolio(CreatePortfolioRequest portfolioCreate)
    {
        var portfolio = _mapper.Map<Portfolio>(portfolioCreate);
        var hasCustomerBankInfo = _customerBankInfoAppService.GetAllCustomerBankInfo();
        
        foreach (var customerBankInfo in hasCustomerBankInfo)
        {
            if (customerBankInfo.CustomerId != portfolio.CustomerId) 
                throw new CustomerException($"Não é possível criar portfolio para o Customer com Id: {portfolio.CustomerId}");
        }

        return _portfolioService.CreatePortfolio(portfolio);
    }

    public IEnumerable<PortfolioResult> GetAllPortfolios()
    {
        var portfoliosFound = _portfolioService.GetAllPortfolios();
        var portfolioMapp = _mapper.Map<IEnumerable<PortfolioResult>>(portfoliosFound);

        foreach (var portfolios in portfoliosFound)
        {
            foreach (var product in portfolioMapp)
            {
                product.Products = _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolios.PortfolioProducts);
            }
        }

        return portfolioMapp;
    }

    public PortfolioResult GetPortfolioById(long id)
    {
        var portfolioFound = _portfolioService.GetPortfolioById(id);

        if (portfolioFound is null) throw new CustomerException($"Portfolio for Id: {id} not found.");

        var portfolioMapp = _mapper.Map<PortfolioResult>(portfolioFound);
        portfolioMapp.Products = _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolioFound.PortfolioProducts);

        return portfolioMapp;
    }

    public decimal GetTotalBalance(long portfolioId)
    {
        return _portfolioService.GetTotalBalance(portfolioId);
    }

    public bool UpdateBalanceAfterPurchase(PortfolioResult portfolioResult, decimal purchaseValue)
    {
        var portfolioToUpdate = _mapper.Map<Portfolio>(portfolioResult);
        return _portfolioService.UpdateBalanceAfterPurchase(portfolioToUpdate);
    }

    public void Delete(long id)
    {
        var portfolios = _portfolioService.GetAllPortfolios();
        var portfolioToDelete = new Portfolio();

        foreach (var portfolio in portfolios)
        {
            if (portfolio.Id.Equals(id)) portfolioToDelete = portfolio;
        }

        var portfolioTotalBalance = _portfolioService.GetTotalBalance(id);
        var customerBankInfos = _customerBankInfoAppService.GetAllCustomerBankInfo();

        foreach (var customerBankInfo in customerBankInfos)
        {
            if (customerBankInfo.CustomerId == portfolioToDelete.CustomerId) 
                _customerBankInfoAppService.UpdateBalanceAfterRescue(customerBankInfo, portfolioTotalBalance);
        }

        _portfolioService.Delete(id);
    }

    public long Invest(CreateInvestmentRequest request, long customerBankId)
    {
        var productFound = ProductExists(request.ProductId);
        var portfolioFound = PortfolioExists(request.PortfolioId);
        var customerBankInfoFound = _customerBankInfoAppService.GetCustomerBankInfoById(customerBankId);
        var investment = _mapper.Map<PortfolioProduct>(request);

        CheckCustomerAccountBalance(investment, customerBankInfoFound.AccountBalance, productFound);

        var createdInvestment = _portfolioService.Add(investment);

        PostInvestmentUpdates(portfolioFound, customerBankInfoFound, investment);

        return createdInvestment;
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
        var portfolioFound = GetPortfolioById(portfolioId);

        if (portfolioFound is null)
            throw new CustomerException($"Portfolio not found to the Id: {portfolioId}");

        return portfolioFound;
    }

    private void PostInvestmentUpdates(PortfolioResult portfolioFound, CustomerBankInfoResult customerBankInfoFound, PortfolioProduct investment)
    {
        portfolioFound.TotalBalance += investment.NetValue;
        var portfolioToUpdate = _mapper.Map<Portfolio>(portfolioFound);
        _portfolioService.UpdateBalanceAfterPurchase(portfolioToUpdate);
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
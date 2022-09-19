using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using Application.Models.PortfolioProduct.Response;
using Application.Models.Product.Response;
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
                throw new GenericNotFoundException($"Unable to create portfolio for the Customer with Id: {portfolio.CustomerId} not found");
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

        if (portfolioFound is null) throw new GenericNotFoundException($"Portfolio for Id: {id} not found.");

        var portfolioMapp = _mapper.Map<PortfolioResult>(portfolioFound);
        portfolioMapp.Products = _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolioFound.PortfolioProducts);

        return portfolioMapp;
    }

    private Portfolio GetById(long id)
    {
        var portfolioFound = _portfolioService.GetPortfolioById(id);

        if (portfolioFound is null) throw new GenericNotFoundException($"Portfolio for Id: {id} not found.");

        return portfolioFound;
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
        _customerBankInfoAppService.RedeemInvestedAmount(portfolioToDelete.CustomerId, portfolioTotalBalance);

        _portfolioService.Delete(id);
    }

    public long Invest(CreateInvestmentRequest request, long customerBankId)
    {
        var productFound = _productAppService.GetProductById(request.ProductId);
        var portfolioFound = GetById(request.PortfolioId);
        var investment = _mapper.Map<PortfolioProduct>(request);

        CheckCustomerAccountBalance(investment, customerBankId, productFound);

        var createdInvestment = _portfolioService.Add(investment);

        PostInvestmentUpdates(portfolioFound, customerBankId, investment);

        return createdInvestment;
    }

    private void PostInvestmentUpdates(Portfolio portfolioFound, long customerBankId, PortfolioProduct investment)
    {
        portfolioFound.TotalBalance += investment.NetValue;
        _portfolioService.UpdateBalanceAfterPurchase(portfolioFound);
        _customerBankInfoAppService.UpdateBalanceAfterPurchase(customerBankId, investment.NetValue);
    }

    private bool CheckCustomerAccountBalance(PortfolioProduct investment, long customerBankId, ProductResult productFound)
    {
        investment.NetValue = productFound.UnitPrice * investment.Quotes;
        var customerAccountBalance = _customerBankInfoAppService.CheckCustomerAccountBalance(investment.NetValue, customerBankId);

        if (customerAccountBalance is false ) 
            throw new GenericBalancesException($"Insufficient balance to invest.");

        return true;
    }
}
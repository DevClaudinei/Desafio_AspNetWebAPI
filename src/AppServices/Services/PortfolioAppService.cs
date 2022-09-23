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
    private readonly IPortfolioProductAppService _portfolioProductAppService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

    public PortfolioAppService(
        IMapper mapper,
        IPortfolioService portfolioService,
        IProductAppService productAppService,
        ICustomerBankInfoAppService customerBankInfoAppService,
        IPortfolioProductAppService portfolioProductAppService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
        _portfolioProductAppService = portfolioProductAppService ?? throw new ArgumentNullException(nameof(portfolioProductAppService));
    }

    public long Create(CreatePortfolioRequest portfolioCreate)
    {
        var portfolio = _mapper.Map<Portfolio>(portfolioCreate);
        var hasCustomerBankInfo = _customerBankInfoAppService.GetAll();
        
        foreach (var customerBankInfo in hasCustomerBankInfo)
        {
            if (customerBankInfo.CustomerId != portfolio.CustomerId) 
                throw new NotFoundException($"Unable to create portfolio for the Customer with Id: {portfolio.CustomerId} not found");
        }

        return _portfolioService.Create(portfolio);
    }

    public IEnumerable<PortfolioResult> GetAll()
    {
        var portfoliosFound = _portfolioService.GetAll();
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
        var portfolioFound = _portfolioService.GetById(id);

        if (portfolioFound is null) throw new NotFoundException($"Portfolio for Id: {id} not found.");

        var portfolioMapp = _mapper.Map<PortfolioResult>(portfolioFound);
        portfolioMapp.Products = _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolioFound.PortfolioProducts);

        return portfolioMapp;
    }

    private Portfolio GetById(long id)
    {
        var portfolioFound = _portfolioService.GetById(id);

        if (portfolioFound is null) throw new NotFoundException($"Portfolio for Id: {id} not found.");

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
        var portfolios = _portfolioService.GetAll();
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
        var productFound = _productAppService.GetById(request.ProductId);
        var portfolioFound = GetById(request.PortfolioId);
        var investment = _mapper.Map<PortfolioProduct>(request);

        CheckCustomerAccountBalance(investment, customerBankId, productFound);

        var createdInvestment = _portfolioProductAppService.Create(investment);

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
            throw new BadRequestException($"Insufficient balance to invest.");

        return true;
    }
}
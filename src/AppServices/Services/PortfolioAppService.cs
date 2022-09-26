using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
using Application.Models.PortfolioProduct.Response;
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
        var portfolio = _portfolioService.GetById(id);

        var portfolioTotalBalance = _portfolioService.GetTotalBalance(id);
        _customerBankInfoAppService.RedeemInvestedAmount(portfolio.CustomerId, portfolioTotalBalance);

        _portfolioService.Delete(id);
    }

    public long Invest(InvestmentRequest request)
    {
        var portfolioFound = GetById(request.PortfolioId);
        var productFound = _productAppService.GetById(request.ProductId);
        var portfolioProduct = new PortfolioProduct(productFound.UnitPrice, request.Quotes);
        var investment = _mapper.Map<PortfolioProduct>(request);
        investment.NetValue = portfolioProduct.NetValue;
        var customerBankId = _customerBankInfoAppService.GetCustomerBankInfoId(portfolioFound.CustomerId);

        CheckCustomerAccountBalance(investment.NetValue, customerBankId);

        if (!(request.ConvertedAt <= DateTime.UtcNow)) throw new BadRequestException("Não é possível investir com data futura.");

        var createdInvestment = _portfolioProductAppService.Create(investment);
        PostInvestmentUpdates(portfolioFound, customerBankId, investment.NetValue);

        return createdInvestment;
    }

    public long WithdrawInvestment( WithdrawInvestmentRequest request)
    {
        var portfolioFound = GetById(request.PortfolioId);
        var productFound = _productAppService.GetById(request.ProductId);
        var portfolioProduct = new PortfolioProduct(productFound.UnitPrice, request.Quotes);
        var investment = _mapper.Map<PortfolioProduct>(request);
        investment.NetValue = portfolioProduct.NetValue;
        var customerBankId = _customerBankInfoAppService.GetCustomerBankInfoId(portfolioFound.CustomerId);
        var totalDeCotasNaCarteira = _portfolioProductAppService
            .GetQuantityOfQuotes(portfolioFound.Id, request.ProductId, request.Quotes);
        

        if (totalDeCotasNaCarteira < request.Quotes) throw new BadRequestException("Não é possível sacar o investimento.");

        var withdrawInvestment = _portfolioProductAppService.Create(investment);
        PostInvestmentUpdates(portfolioFound, customerBankId, -investment.NetValue);
        _portfolioProductAppService.RemoveProduct(portfolioFound, productFound);
        return withdrawInvestment;
    }

    private void PostInvestmentUpdates(Portfolio portfolioFound, long customerBankId, decimal netValue)
    {
        portfolioFound.TotalBalance += netValue;
        _portfolioService.UpdateBalanceAfterPurchase(portfolioFound);
        _customerBankInfoAppService.UpdateBalanceAfterPurchase(customerBankId, netValue);
    }

    private void CheckCustomerAccountBalance(decimal netValue, long customerBankId)
    {
        var customerAccountBalance = _customerBankInfoAppService
            .CanWithdrawAmountFromAccountBalance(netValue, customerBankId);

        if (customerAccountBalance is false ) 
            throw new BadRequestException($"Insufficient balance to invest.");
    }
}
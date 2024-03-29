﻿using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
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
    private readonly IOrderAppService _orderAppService;
    private readonly IPortfolioService _portfolioService;
    private readonly IProductAppService _productAppService;
    private readonly IPortfolioProductAppService _portfolioProductAppService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

    public PortfolioAppService(
        IMapper mapper,
        IOrderAppService orderAppService,
        IPortfolioService portfolioService,
        IProductAppService productAppService,
        IPortfolioProductAppService portfolioProductAppService,
        ICustomerBankInfoAppService customerBankInfoAppService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderAppService = orderAppService ?? throw new ArgumentNullException(nameof(orderAppService));
        _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        _portfolioProductAppService = portfolioProductAppService ?? throw new ArgumentNullException(nameof(portfolioProductAppService));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
    }

    public long Create(CreatePortfolioRequest createPortfolioRequest)
    {
        var portfolio = _mapper.Map<Portfolio>(createPortfolioRequest);
        var customerBankInfoId = _customerBankInfoAppService.GetCustomerBankInfoId(createPortfolioRequest.CustomerId);

        if (customerBankInfoId < 1) throw new NotFoundException($"Customer for Id: {portfolio.CustomerId} not found");

        return _portfolioService.Create(portfolio);
    }

    public IEnumerable<PortfolioResult> GetAll()
    {
        var portfoliosFound = _portfolioService.GetAll();
        var portfolioMap = _mapper.Map<IEnumerable<PortfolioResult>>(portfoliosFound);

        return portfolioMap;
    }

    public PortfolioResult GetPortfolioById(long id)
    {
        var portfolioFound = _portfolioService.GetById(id)
            ?? throw new NotFoundException($"Portfolio for Id: {id} not found.");

        var portfolioMapp = _mapper.Map<PortfolioResult>(portfolioFound);

        return portfolioMapp;
    }

    private Portfolio GetById(long id)
    {
        var portfolioFound = _portfolioService.GetById(id)
            ?? throw new NotFoundException($"Portfolio for Id: {id} not found.");

        return portfolioFound;
    }

    public decimal GetTotalBalance(long id)
    {
        GetById(id);

        return _portfolioService.GetTotalBalance(id);
    }

    public void Delete(long id)
    {
        var portfolio = _portfolioService.GetById(id)
            ?? throw new NotFoundException($"Portfolio for Id: {id} not found.");

        var portfolioTotalBalance = _portfolioService.GetTotalBalance(id);
        _customerBankInfoAppService.Withdraw(portfolio.CustomerId, portfolioTotalBalance);

        _portfolioService.Delete(id);
    }

    public long Invest(InvestmentRequest request)
    {
        var portfolioFound = GetById(request.PortfolioId);
        var productFound = _productAppService.Get(request.ProductId);
        var investment = _mapper.Map<Order>(request);
        investment.NetValue = productFound.UnitPrice * request.Quotes;

        InvestimentRealize(investment, portfolioFound, productFound);

        var createdInvestment = _orderAppService.Create(investment);
        return createdInvestment;
    }

    public long Uninvest(UninvestimentRequest request)
    {
        var portfolioFound = GetById(request.PortfolioId);
        var productFound = _productAppService.Get(request.ProductId);
        var investment = _mapper.Map<Order>(request);
        investment.NetValue = productFound.UnitPrice * request.Quotes;

        UninvestimentRealize(investment, portfolioFound, productFound);

        var UnivestmentId = _orderAppService.Create(investment);
        return UnivestmentId;
    }

    private void InvestimentRealize(Order order, Portfolio portfolio, Product product)
    {
        var customerBankId = CheckCustomerAccountBalance(portfolio.CustomerId, order);

        PostInvestmentUpdates(portfolio, customerBankId, order.NetValue);
        _portfolioProductAppService.AddProduct(portfolio, product);
    }

    private long CheckCustomerAccountBalance(long customerId, Order order)
    {
        var customerBankId = _customerBankInfoAppService.GetCustomerBankInfoId(customerId);
        _customerBankInfoAppService
            .CanWithdrawAmountFromAccountBalance(order.NetValue, customerBankId);

        return customerBankId;
    }

    private void PostInvestmentUpdates(Portfolio portfolio, long customerBankId, decimal netValue)
    {
        portfolio.TotalBalance += netValue;
        _portfolioService.Update(portfolio);
        _customerBankInfoAppService.Withdraw(customerBankId, netValue);
    }

    private void UninvestimentRealize(Order order, Portfolio portfolio, Product product)
    {
        var customerBankId = _customerBankInfoAppService.GetCustomerBankInfoId(portfolio.CustomerId);

        CheckCustomerQuotes(portfolio, product);
        PostUninvestmentUpdates(portfolio, customerBankId, order.NetValue);
    }

    private void CheckCustomerQuotes(Portfolio portfolio, Product product)
    {
        var quantityCotes = _orderAppService.GetQuantityOfQuotes(portfolio.Id, product.Id);

        if (quantityCotes == 0)
            _portfolioProductAppService.RemoveProduct(portfolio, product);
    }

    private void PostUninvestmentUpdates(Portfolio portfolio, long customerBankId, decimal netValue)
    {
        portfolio.TotalBalance -= netValue;
        _portfolioService.Update(portfolio);
        _customerBankInfoAppService.Deposit(customerBankId, netValue);
    }

    public IEnumerable<PortfolioResult> GetAllByCustomerId(long id)
    {
        var portfoliosFound = _portfolioService.GetAllByCustomerId(id);
        var portfolioMap = _mapper.Map<IEnumerable<PortfolioResult>>(portfoliosFound);

        return portfolioMap;
    }
}
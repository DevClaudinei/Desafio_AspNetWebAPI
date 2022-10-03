using Application.Models.Enum;
using Application.Models.Portfolio.Request;
using Application.Models.Portfolio.Response;
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
    private readonly IOrderAppService _orderAppService;
    private readonly IPortfolioService _portfolioService;
    private readonly IProductAppService _productAppService;
    private readonly IPortfolioProductAppService _portfolioProductAppService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

    public PortfolioAppService(
        IMapper mapper,
        IPortfolioService portfolioService,
        IProductAppService productAppService,
        IOrderAppService orderAppService,
        IPortfolioProductAppService portfolioProductAppService,
        ICustomerBankInfoAppService customerBankInfoAppService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
        _orderAppService = orderAppService ?? throw new ArgumentNullException(nameof(orderAppService));
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
                product.Products = _mapper.Map<IEnumerable<ProductResult>>(portfolios.Products);
            }
        }

        return portfolioMapp;
    }

    public PortfolioResult GetPortfolioById(long id)
    {
        var portfolioFound = _portfolioService.GetById(id);

        if (portfolioFound is null) throw new NotFoundException($"Portfolio for Id: {id} not found.");

        var portfolioMapp = _mapper.Map<PortfolioResult>(portfolioFound);
        portfolioMapp.Products = _mapper.Map<IEnumerable<ProductResult>>(portfolioFound.Products);

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
        return _portfolioService.Update(portfolioToUpdate);
    }

    public void Delete(long id)
    {
        var portfolio = _portfolioService.GetById(id);

        var portfolioTotalBalance = _portfolioService.GetTotalBalance(id);
        _customerBankInfoAppService.Withdraw(portfolio.CustomerId, portfolioTotalBalance);

        _portfolioService.Delete(id);
    }

    public long Invest(InvestmentRequest request, OrderDirection orderDirection)
    {
        var portfolioFound = GetById(request.PortfolioId);
        var productFound = _productAppService.Get(request.ProductId);
        var order = PrepareOrder(productFound.UnitPrice, request, orderDirection);
        var customerBankId = CheckCustomerAccountBalance(portfolioFound.CustomerId, order);
        var createdInvestment = _orderAppService.Create(order);

        if (orderDirection != OrderDirection.Buy)
            UninvestimentRealize(order, customerBankId, portfolioFound, productFound);

        if (orderDirection == OrderDirection.Buy)
            InvestimentRealize(order, customerBankId, portfolioFound, productFound);

        return createdInvestment;
    }

    private Order PrepareOrder(decimal unitPrice, InvestmentRequest request, OrderDirection orderDirection)
    {
        var order = new Order(unitPrice, request.Quotes);
 
        var investment = _mapper.Map<Order>(request);
        investment.NetValue = order.NetValue;

        return investment;
    }

    private long CheckCustomerAccountBalance(long customerId, Order order)
    {
        var customerBankId = _customerBankInfoAppService.GetCustomerBankInfoId(customerId);
        var customerAccountBalance = _customerBankInfoAppService
            .CanWithdrawAmountFromAccountBalance(order.NetValue, customerBankId);

        if (customerAccountBalance is false)
            throw new BadRequestException($"Insufficient balance to invest.");

        return customerBankId;
    }

    private void UninvestimentRealize(Order order, long customerBankId, Portfolio portfolioFound, Product productFound)
    {
        CheckCustomerQuotes(portfolioFound, productFound);
        PostInvestmentUpdates(portfolioFound, customerBankId, - order.NetValue);
    }   

    private void InvestimentRealize(Order order, long customerBankId, Portfolio portfolioFound, Product productFound)
    {
        if (!(order.ConvertedAt <= DateTime.UtcNow)) 
            throw new BadRequestException("It is not possible to make an investment with a future date.");

        PostInvestmentUpdates(portfolioFound, customerBankId, order.NetValue);
        _portfolioProductAppService.AddProduct(portfolioFound, productFound);
    }

    private bool CheckCustomerQuotes(Portfolio portfolioFound, Product productFound)
    {
        var quantityCotes = _orderAppService.GetQuantityOfQuotes(portfolioFound.Id, productFound.Id);

        if(quantityCotes == 0)
            _portfolioProductAppService.RemoveProduct(portfolioFound, productFound);

        return true;
    }

    private void PostInvestmentUpdates(Portfolio portfolioFound, long customerBankId, decimal netValue)
    {
        portfolioFound.TotalBalance += netValue;
        _portfolioService.Update(portfolioFound);
        _customerBankInfoAppService.Withdraw(customerBankId, -netValue);
    }
}
using Application.Models.Order;
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
    private readonly IPortfolioService _portfolioService;
    private readonly IProductAppService _productAppService;
    private readonly IOrderAppService _orderAppService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

    public PortfolioAppService(
        IMapper mapper,
        IPortfolioService portfolioService,
        IProductAppService productAppService,
        ICustomerBankInfoAppService customerBankInfoAppService,
        IOrderAppService portfolioProductAppService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _portfolioService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
        _orderAppService = portfolioProductAppService ?? throw new ArgumentNullException(nameof(portfolioProductAppService));
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
        _customerBankInfoAppService.RedeemInvestedAmount(portfolio.CustomerId, portfolioTotalBalance);

        _portfolioService.Delete(id);
    }

    public long Invest(InvestmentRequest request)
    {
        var portfolioFound = GetById(request.PortfolioId);
        var productFound = _productAppService.Get(request.ProductId);
        var order = new Order(productFound.UnitPrice, request.Quotes);
        var investment = _mapper.Map<Order>(request);
        investment.NetValue = order.NetValue;
        var customerBankId = _customerBankInfoAppService.GetCustomerBankInfoId(portfolioFound.CustomerId);

        CheckCustomerAccountBalance(investment.NetValue, customerBankId);

        if (!(request.ConvertedAt <= DateTime.UtcNow)) throw new BadRequestException("Não é possível investir com data futura.");

        var createdInvestment = _orderAppService.Create(investment);

        PostInvestmentUpdates(portfolioFound, customerBankId, investment.NetValue);

        _portfolioService.AddProduct(portfolioFound, productFound);

        return createdInvestment;
    }

    private void PostInvestmentUpdates(Portfolio portfolioFound, long customerBankId, decimal netValue)
    {
        portfolioFound.TotalBalance += netValue;
        _portfolioService.Update(portfolioFound);
        _customerBankInfoAppService.UpdateBalanceAfterPurchase(customerBankId, -netValue);
    }

    private void CheckCustomerAccountBalance(decimal netValue, long customerBankId)
    {
        var customerAccountBalance = _customerBankInfoAppService
            .CanWithdrawAmountFromAccountBalance(netValue, customerBankId);

        if (customerAccountBalance is false)
            throw new BadRequestException($"Insufficient balance to invest.");
    }

    private OrderResult VerificarRemocaoDeCotas(IEnumerable<OrderResult> orders, WithdrawInvestmentRequest request)
    {
        var orderResult = new OrderResult();

        foreach (var order in orders)
        {
            if (order.Quotes - request.Quotes > 0)
            { 
                orderResult = order;
                orderResult.Quotes -= request.Quotes;
                orderResult.NetValue = orderResult.NetValue / request.Quotes;
                break;
            }

            if (request.Quotes > order.Quotes) throw new BadRequestException("O numero de cotas solicitadas é maior que a existente.");
        }

        return orderResult;
    }

    public long WithdrawInvestment(WithdrawInvestmentRequest request)
    {
        var portfolioFound = GetById(request.PortfolioId);
        var productFound = _productAppService.Get(request.ProductId);
        var portfolioProduct = new Order(productFound.UnitPrice, request.Quotes);
        var investment = _mapper.Map<Order>(request);
        
        investment.NetValue = portfolioProduct.NetValue;
        
        var customerBankId = _customerBankInfoAppService.GetCustomerBankInfoId(portfolioFound.CustomerId);
        var orders = _orderAppService
            .GetQuantityOfQuotes(portfolioFound.Id, request.ProductId);
        var order = VerificarRemocaoDeCotas(orders, request);

        PostInvestmentUpdates(portfolioFound, customerBankId, -investment.NetValue);

        var withdrawInvestment = _orderAppService.Create(investment);
        _orderAppService.Update(order.Id, order, productFound.Id, portfolioFound.Id);

        if (order.Quotes == 0)
            _portfolioService.RemoveProduct(portfolioFound, productFound);

        return withdrawInvestment;
    }
}
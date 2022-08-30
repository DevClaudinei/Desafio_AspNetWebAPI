using Application.Models.PortfolioProduct.Request;
using Application.Models.PortfolioProduct.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
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
    private readonly ICustomerAppService _customerAppService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

    public PortfolioProductAppService(
        IMapper mapper,
        IPortfolioProductService portfolioProductService,
        IPortfolioAppService portfolioAppService,
        IProductAppService productAppService,
        ICustomerBankInfoAppService customerBankInfoAppService,
        ICustomerAppService customerAppService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));
        _portfolioAppService = portfolioAppService ?? throw new ArgumentNullException(nameof(portfolioAppService));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
        _customerAppService = customerAppService ?? throw new ArgumentNullException(nameof(customerAppService));
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

    public (bool, string) Invest(InvestmentRequest request, long customerBankId)
    {
        var productFound = _productAppService.GetProductById(request.ProductId);
        var portfolioFound = _portfolioAppService.GetPortfolioById(request.PortfolioId);
        var customerBankInfoFound = _customerBankInfoAppService.GetCustomerBankInfoById(customerBankId);

        if (productFound is null || portfolioFound is null) return (false, $"Produto não encontrado para o Id: {request.ProductId}");

        var investment = _mapper.Map<PortfolioProduct>(request);
        investment.NetValue = productFound.UnitPrice * investment.Quotes;

        if (customerBankInfoFound.AccountBalance < investment.NetValue) 
            return (false, $"Saldo insuficiente para adquirir o produto com o Id: {productFound.Id}");

        var createdInvestment = _portfolioProductService.Add(investment);

        if (createdInvestment.isValid)
        {
            portfolioFound.TotalBalance += investment.NetValue;
            _portfolioAppService.UpdateBalanceAfterPurchase(portfolioFound, investment.NetValue);
            _customerBankInfoAppService.UpdateBalanceAfterPurchase(customerBankInfoFound, investment.NetValue);
            return (true, createdInvestment.message);
        } 
            
        return (false, createdInvestment.message);
    }
}

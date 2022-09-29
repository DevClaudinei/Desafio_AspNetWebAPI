//using Application.Models.Portfolio.Request;
//using Application.Models.PortfolioProduct.Response;
//using Application.Models.Product.Response;
//using AppServices.Services.Interfaces;
//using AutoMapper;
//using DomainModels.Entities;
//using DomainServices.Exceptions;
//using DomainServices.Services.Interfaces;
//using System;
//using System.Collections.Generic;

//namespace AppServices.Services;

//public class PortfolioProductAppService : IOrderAppService
//{
//    private readonly IMapper _mapper;
//    private readonly IPortfolioProductService _portfolioProductService;
    
//    public PortfolioProductAppService(IMapper mapper, IPortfolioProductService portfolioProductService)
//    {
//        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
//        _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));
//    }

//    public long Create(Order investment)
//    {
//        return _portfolioProductService.Create(investment);
//    }

//    public IEnumerable<OrderResult> GetAllOrder()
//    {
//        var portfolioProductsFound = _portfolioProductService.GetAll();
//        return _mapper.Map<IEnumerable<OrderResult>>(portfolioProductsFound);
//    }

//    public OrderResult GetOrderById(long id)
//    {
//        var portfolioProductFound = _portfolioProductService.GetById(id);
//        if (portfolioProductFound is null) throw new NotFoundException($"PortfolioProduct for id: {id} not found.");

//        return _mapper.Map<OrderResult>(portfolioProductFound);
//    }

//    public void WithdrawInvestment(long id, WithdrawInvestmentRequest withdrawInvestment)
//    {
//        var portfolioProductToUpdate = _mapper.Map<Order>(withdrawInvestment);
//        _portfolioProductService.WithdrawInvestment(id, portfolioProductToUpdate);
//    }

//    public int GetQuantityOfQuotes(long portfolioId, long productId, int quotes)
//    {
//        return _portfolioProductService.GetQuantityOfQuotes(portfolioId, productId, quotes);
//    }

//    public void RemoveProduct(Portfolio portfolioFound, Product productFound)
//    {
//        _portfolioProductService.RemoveProduct(portfolioFound, productFound);
//    }
//}
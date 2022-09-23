using Application.Models.PortfolioProduct.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class PortfolioProductAppService : IPortfolioProductAppService
{
    private readonly IMapper _mapper;
    private readonly IPortfolioProductService _portfolioProductService;
    
    public PortfolioProductAppService(
        IMapper mapper,
        IPortfolioProductService portfolioProductService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));
    }

    public long Create(PortfolioProduct investment)
    {
        return _portfolioProductService.Create(investment);
    }

    public IEnumerable<PortfolioProductResult> GetAllPortfolioProduct()
    {
        var portfolioProductsFound = _portfolioProductService.GetAll();
        return _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolioProductsFound);
    }

    public PortfolioProductResult GetPortfolioProductById(long id)
    {
        var portfolioProductFound = _portfolioProductService.GetById(id);
        if (portfolioProductFound is null) throw new NotFoundException($"PortfolioProduct for id: {id} not found.");

        return _mapper.Map<PortfolioProductResult>(portfolioProductFound);
    }
}
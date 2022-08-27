using Application.Models;
using Application.Models.PortfolioProduct;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainServices;
using DomainServices.Services;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class PortfolioProductAppService : IPortfolioProductAppService
{
    private readonly IMapper _mapper;
    private readonly IPortfolioProductService _portfolioProductService;

    public PortfolioProductAppService(IMapper mapper, IPortfolioProductService portfolioProductService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));
    }

    public IEnumerable<PortfolioProductResult> GetAllPortfolioProduct()
    {
        var portfolioProductsFound = _portfolioProductService.GetAllPortfolioProduct();
        return _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolioProductsFound);
    }

    public PortfolioProductResult GetPortfolioProductById(Guid id)
    {
        var portfolioProductFound = _portfolioProductService.GetPortfolioProductById(id);
        if (portfolioProductFound is null) return null;

        return _mapper.Map<PortfolioProductResult>(portfolioProductFound);
    }
}

using Application.Models.PortfolioProduct.Response;
using Application.Models.Product.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class PortfolioProductAppService : IPortfolioProductAppService
{
    private readonly IMapper _mapper;
    private readonly IProductAppService _productAppService;
    private readonly IPortfolioProductService _portfolioProductService;
    
    public PortfolioProductAppService(
        IMapper mapper,
        IProductAppService productAppService,
        IPortfolioProductService portfolioProductService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        _portfolioProductService = portfolioProductService ?? throw new ArgumentNullException(nameof(portfolioProductService));
    }

    public IEnumerable<PortfolioProductResult> GetAllPortfolioProduct()
    {
        var portfolioProductsFound = _portfolioProductService.GetAllPortfolioProduct();
        return _mapper.Map<IEnumerable<PortfolioProductResult>>(portfolioProductsFound);
    }

    public PortfolioProductResult GetPortfolioProductById(long id)
    {
        var portfolioProductFound = _portfolioProductService.GetPortfolioProductById(id);
        if (portfolioProductFound is null) throw new CustomerException($"PortfolioProduct for id: {id} not found.");

        return _mapper.Map<PortfolioProductResult>(portfolioProductFound);
    }
}
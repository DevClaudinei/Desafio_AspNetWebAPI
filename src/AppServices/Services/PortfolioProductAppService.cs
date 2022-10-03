using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using System.Collections.Generic;

namespace AppServices.Services;

public class PortfolioProductAppService : IPortfolioProductAppService
{
    private readonly IMapper _mapper;
    private readonly IPortfolioProductService _portfolioProductService;

    public PortfolioProductAppService(IMapper mapper, IPortfolioProductService portfolioProductService)
    {
        _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        _portfolioProductService = portfolioProductService ?? throw new System.ArgumentNullException(nameof(portfolioProductService));
    }

    public IEnumerable<PortfolioProduct> GetAll()
    {
        var portfolioProducts = _portfolioProductService.GetAll();
        return _mapper.Map<IEnumerable<PortfolioProduct>>(portfolioProducts);
    }

    public PortfolioProduct GetById(long portfolioId, long productId)
    {
        var portfolioProduct = _portfolioProductService.GetById(portfolioId, productId);
        if (portfolioProduct is null)
            throw new NotFoundException($"PortfolioProduct with Portfolioid: {portfolioId} and ProductId: {productId} not found.");
        
        return portfolioProduct;
    }

    public void AddProduct(Portfolio portfolio, Product product)
    {
        _portfolioProductService.AddProduct(portfolio, product);
    }

    public void RemoveProduct(Portfolio portfolio, Product product)
    {
        _portfolioProductService.RemoveProduct(portfolio, product);
    }
}
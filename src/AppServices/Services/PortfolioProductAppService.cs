using AppServices.Services.Interfaces;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using System.Collections.Generic;

namespace AppServices.Services;

public class PortfolioProductAppService : IPortfolioProductAppService
{
    private readonly IPortfolioProductService _portfolioProductService;

    public PortfolioProductAppService(IPortfolioProductService portfolioProductService)
    {
        _portfolioProductService = portfolioProductService ?? throw new System.ArgumentNullException(nameof(portfolioProductService));
    }

    public IEnumerable<PortfolioProduct> GetAll()
    {
        var portfolioProducts = _portfolioProductService.GetAll();
        return portfolioProducts;
    }

    public PortfolioProduct GetById(long portfolioId, long productId)
    {
        var portfolioProduct = _portfolioProductService.GetById(portfolioId, productId)
            ?? throw new NotFoundException($"PortfolioProduct for Portfolioid: {portfolioId} and ProductId: {productId} not found.");

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
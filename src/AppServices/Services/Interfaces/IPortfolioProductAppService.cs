using Application.Models.PortfolioProduct.Response;
using Application.Models.Product.Response;
using DomainModels.Entities;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioProductAppService
{
    long Create(PortfolioProduct investment);
    IEnumerable<PortfolioProductResult> GetAll();
    PortfolioProductResult GetById(long id);
    int GetQuantityOfQuotes(long portfolioId, long productId, int quotes);
    void RemoveProduct(Portfolio portfolioFound, ProductResult productFound);
}
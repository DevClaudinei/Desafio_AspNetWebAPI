using Application.Models.Product.Response;
using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioProductService
{
    long Create(PortfolioProduct portfolioProduct);
    IEnumerable<PortfolioProduct> GetAll();
    PortfolioProduct GetById(long id);
    int GetQuantityOfQuotes(long portfolioId, long productId, int quotes);
    void RemoveProduct(Portfolio portfolioFound, ProductResult productFound);
    void WithdrawInvestment(long id, PortfolioProduct portfolioProductToUpdate);
}
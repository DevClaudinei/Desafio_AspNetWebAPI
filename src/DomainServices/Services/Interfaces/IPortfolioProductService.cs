using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioProductService
{
    void AddProduct(Portfolio portfolio, Product product);
    IEnumerable<PortfolioProduct> GetAll();
    PortfolioProduct GetById(long portfolioId, long productId);
    void RemoveProduct(Portfolio portfolio, Product product);
}
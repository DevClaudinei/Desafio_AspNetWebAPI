using DomainModels.Entities;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IPortfolioProductAppService
{
    IEnumerable<PortfolioProduct> GetAll();
    PortfolioProduct GetById(long portfolioId, long productId);
    void AddProduct(Portfolio portfolio, Product product);
    void RemoveProduct(Portfolio portfolio, Product product);
}
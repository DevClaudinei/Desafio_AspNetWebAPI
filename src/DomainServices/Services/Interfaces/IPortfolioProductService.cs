using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioProductService
{
    long Add(PortfolioProduct portfolioProduct);
    IEnumerable<PortfolioProduct> GetAllPortfolioProduct();
    PortfolioProduct GetPortfolioProductById(long id);
}
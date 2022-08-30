using System.Collections.Generic;
using System;
using DomainModels.Entities;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioProductService
{
    IEnumerable<PortfolioProduct> GetAllPortfolioProduct();
    PortfolioProduct GetPortfolioProductById(long id);
    (bool isValid, string message) Add(PortfolioProduct portfolioProduct);
}

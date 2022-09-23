using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IPortfolioProductService
{
    long Create(PortfolioProduct portfolioProduct);
    IEnumerable<PortfolioProduct> GetAll();
    PortfolioProduct GetById(long id);
}
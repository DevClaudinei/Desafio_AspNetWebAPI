using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;

namespace DomainServices.Services;

public class PortfolioProductService : IPortfolioProductService
{
    private readonly IRepositoryFactory _repositoryFactory;

    public PortfolioProductService(IRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public IEnumerable<PortfolioProduct> GetAllPortfolioProduct()
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public PortfolioProduct GetPortfolioProductById(long id)
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var customerFound = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(customerFound);
    }
}
using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DomainServices.Services;

public class PortfolioProductService : BaseService, IPortfolioProductService
{
    public PortfolioProductService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory) { }

    public void AddProduct(Portfolio portfolio, Product product)
    {
        var unitOfWork = UnitOfWork.Repository<PortfolioProduct>();
        var productInThePortfolio = GetById(portfolio.Id, product.Id);

        if (productInThePortfolio == null)
        {
            var portfolioProduct = new PortfolioProduct(portfolio.Id, product.Id);

            unitOfWork.Add(portfolioProduct);
            UnitOfWork.SaveChanges();
        }
    }

    public IEnumerable<PortfolioProduct> GetAll()
    {
        var repository = RepositoryFactory.Repository<PortfolioProduct>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public PortfolioProduct GetById(long portfolioId, long productId)
    {
        var repository = RepositoryFactory.Repository<PortfolioProduct>();
        var query = repository.SingleResultQuery()
            .Include(x => x.Include(x => x.Portfolio))
            .Include(x => x.Include(x => x.Product))
            .AndFilter(x => x.Portfolio.Id.Equals(portfolioId))
            .AndFilter(x => x.ProductId.Equals(productId));

        return repository.SingleOrDefault(query);
    }

    public void RemoveProduct(Portfolio portfolio, Product product)
    {
        var unitOfWork = UnitOfWork.Repository<PortfolioProduct>();
        var portfolioProductToRemove = GetById(portfolio.Id, product.Id);

        unitOfWork.Remove(portfolioProductToRemove);
        UnitOfWork.SaveChanges();
    }
}
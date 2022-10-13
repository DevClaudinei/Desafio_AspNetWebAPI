using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DomainServices.Services;

public class PortfolioProductService : BaseService, IPortfolioProductService
{
    private readonly IRepository<PortfolioProduct> _portfolioProductService;

    public PortfolioProductService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory)
    {
        _portfolioProductService = repositoryFactory.Repository<PortfolioProduct>();
    }

    public void AddProduct(Portfolio portfolio, Product product)
    {
        var repository = UnitOfWork.Repository<PortfolioProduct>();

        if (!_portfolioProductService.Any(x => x.ProductId.Equals(product.Id)))
        {
            var portfolioProduct = new PortfolioProduct(portfolio.Id, product.Id);

            repository.Add(portfolioProduct);
            UnitOfWork.SaveChanges();
        }
    }

    public IEnumerable<PortfolioProduct> GetAll()
    {
        var query = _portfolioProductService.MultipleResultQuery();

        return _portfolioProductService.Search(query);
    }

    public PortfolioProduct GetById(long portfolioId, long productId)
    {
        var query = _portfolioProductService.MultipleResultQuery()
            .Include(x => x.Include(x => x.Portfolio))
            .Include(x => x.Include(x => x.Product))
            .AndFilter(x => x.Portfolio.Id.Equals(portfolioId))
            .AndFilter(x => x.ProductId.Equals(productId));

        return _portfolioProductService.SingleOrDefault(query);
    }

    public void RemoveProduct(Portfolio portfolio, Product product)
    {
        var repository = UnitOfWork.Repository<PortfolioProduct>();
        var portfolioProductToRemove = GetById(portfolio.Id, product.Id);

        repository.Remove(portfolioProductToRemove);
        UnitOfWork.SaveChanges();
    }
}
using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DomainServices.Services;

public class PortfolioProductService : IPortfolioProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public PortfolioProductService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public void AddProduct(Portfolio portfolio, Product product)
    {
        var repository = _unitOfWork.Repository<PortfolioProduct>();

        if (!repository.Any(x => x.ProductId.Equals(product.Id)))
        {
            var portfolioProduct = new PortfolioProduct(portfolio.Id, product.Id);

            repository.Add(portfolioProduct);
            _unitOfWork.SaveChanges();
        }
    }

    public IEnumerable<PortfolioProduct> GetAll()
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public PortfolioProduct GetById(long portfolioId, long productId)
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var query = repository.MultipleResultQuery()
            .Include(x => x.Include(x => x.Portfolio))
            .Include(x => x.Include(x => x.Product))
            .AndFilter(x => x.Portfolio.Id.Equals(portfolioId))
            .AndFilter(x => x.ProductId.Equals(productId));

        return repository.SingleOrDefault(query);
    }

    public void RemoveProduct(Portfolio portfolio, Product product)
    {
        var repository = _unitOfWork.Repository<PortfolioProduct>();
        var portfolioProductToRemove = GetById(portfolio.Id, product.Id);

        repository.Remove(portfolioProductToRemove);
        _unitOfWork.SaveChanges();
    }
}
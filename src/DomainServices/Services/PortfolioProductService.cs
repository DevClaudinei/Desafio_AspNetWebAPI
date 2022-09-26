using Application.Models.Product.Response;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DomainServices.Services;

public class PortfolioProductService : IPortfolioProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public PortfolioProductService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    )
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public long Create(PortfolioProduct portfolioProduct)
    {
        var repository = _unitOfWork.Repository<PortfolioProduct>();
        repository.Add(portfolioProduct);
        _unitOfWork.SaveChanges();

        return portfolioProduct.Id;
    }

    public IEnumerable<PortfolioProduct> GetAll()
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var query = repository.MultipleResultQuery()
            .AndFilter(x => x.Portfolio.Id.Equals(x.PortfolioId));

        return repository.Search(query);
    }

    public PortfolioProduct GetById(long id)
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var customerFound = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(customerFound);

    }

    public int GetQuantityOfQuotes(long portfolioId, long productId, int quotes)
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var query = repository.SingleResultQuery()
            .Select(x => x.Quotes)
            .AndFilter(x => x.ProductId.Equals(productId))
            .AndFilter(x => x.PortfolioId.Equals(portfolioId))
            .AndFilter(x => x.Quotes.Equals(quotes));

        return repository.SingleOrDefault(query);
    }

    public void RemoveProduct(Portfolio portfolioFound, ProductResult productFound)
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        var exists = Exists(x => x.ProductId.Equals(productFound.Id));

        if (!exists) throw new NotFoundException($"Product not found for id: {productFound.Id}.");

        repository.Remove(x => x.Portfolio.PortfolioProducts.Equals(productFound));
    }

    private bool Exists(Expression<Func<PortfolioProduct, bool>> predicate)
    {
        var repository = _repositoryFactory.Repository<PortfolioProduct>();
        return repository.Any(predicate);
    }

    public void WithdrawInvestment(long id, PortfolioProduct portfolioProduct)
    {
        var repository = _unitOfWork.Repository<PortfolioProduct>();
        portfolioProduct.Id = id;
        var updatedPortfolioProduct = GetById(id);
        var portfolioProductToUpdate = CheckIfWithdrawalIsValid(updatedPortfolioProduct, portfolioProduct);

        repository.Update(portfolioProductToUpdate);
        _unitOfWork.SaveChanges();
    }

    private PortfolioProduct CheckIfWithdrawalIsValid(PortfolioProduct portfolioProductToUpdate, PortfolioProduct portfolioProduct)
    {
        if (portfolioProduct.Quotes < 0) throw new BadRequestException($"Unable to make negative withdrawals.");

        portfolioProduct.Quotes = portfolioProductToUpdate.Quotes - portfolioProduct.Quotes;

        if (portfolioProductToUpdate.Quotes <= 0) throw new BadRequestException($"Requested balance cannot be redeemed.");

        return portfolioProduct;
    }
}
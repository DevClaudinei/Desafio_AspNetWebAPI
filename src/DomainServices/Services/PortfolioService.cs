using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DomainServices.Services;

public class PortfolioService : BaseService, IPortfolioService
{
    public PortfolioService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory) { }

    public long Create(Portfolio portfolio)
    {
        var unitOfWork = UnitOfWork.Repository<Portfolio>();

        unitOfWork.Add(portfolio);
        UnitOfWork.SaveChanges();

        return portfolio.Id;
    }

    public decimal GetTotalBalance(long id)
    {
        return GetFieldById<Portfolio, decimal>(id, x => x.TotalBalance);
    }

    public Portfolio GetById(long id)
    {
        var repository = RepositoryFactory.Repository<Portfolio>();
        var query = repository.SingleResultQuery()
            .Include(x => x.Include(x => x.Products))
            .AndFilter(x => x.Id.Equals(id));

        return repository.FirstOrDefault(query);
    }

    public IEnumerable<Portfolio> GetAll()
    {
        var repository = RepositoryFactory.Repository<Portfolio>();
        var query = repository.MultipleResultQuery()
            .Include(x => x.Include(x => x.Products));

        return repository.Search(query);
    }

    public void Update(Portfolio portfolio)
    {
        var unitOfWork = UnitOfWork.Repository<Portfolio>();
        portfolio.Products.Clear();

        unitOfWork.Update(portfolio);
        UnitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var unitOfWork = UnitOfWork.Repository<Portfolio>();
        var totalBalance = GetTotalBalance(id);

        if (totalBalance > 0) 
            throw new BadRequestException($"Unable to delete portfolio, because there is still a balance to withdraw");

        unitOfWork.Remove(x => x.Id.Equals(id));
    }

    public IEnumerable<Portfolio> GetAllByCustomerId(long id)
    {
        var repository = RepositoryFactory.Repository<Portfolio>();
        var query = repository.MultipleResultQuery()
            .Include(x => x.Include(x => x.Products))
            .AndFilter(x => x.CustomerId.Equals(id));

        return repository.Search(query);
    }
}
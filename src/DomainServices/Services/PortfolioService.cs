using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DomainServices.Services;

public class PortfolioService : BaseService, IPortfolioService
{
    private readonly IRepository<Portfolio> _portfolioService;

    public PortfolioService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory)
    {
        _portfolioService = repositoryFactory.Repository<Portfolio>();
    }

    public long Create(Portfolio portfolio)
    {
        var repository = UnitOfWork.Repository<Portfolio>();

        repository.Add(portfolio);
        UnitOfWork.SaveChanges();

        return portfolio.Id;
    }

    public decimal GetTotalBalance(long id)
    {
        return GetFieldById<Portfolio, decimal>(id, x => x.TotalBalance);
    }

    public Portfolio GetById(long id)
    {
        var query = _portfolioService.SingleResultQuery()
            .Include(x => x.Include(x => x.Products))
            .AndFilter(x => x.Id.Equals(id));

        return _portfolioService.FirstOrDefault(query);
    }

    public IEnumerable<Portfolio> GetAll()
    {
        var query = _portfolioService.MultipleResultQuery()
            .Include(x => x.Include(x => x.Products));

        return _portfolioService.Search(query);
    }

    public void Update(Portfolio portfolio)
    {
        var repository = UnitOfWork.Repository<Portfolio>();
        portfolio.Products.Clear();

        repository.Update(portfolio);
        UnitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var repository = UnitOfWork.Repository<Portfolio>();
        var totalBalance = GetTotalBalance(id);

        if (totalBalance > 0) 
            throw new BadRequestException($"Unable to delete portfolio, because there is still a balance to withdraw");

        repository.Remove(x => x.Id.Equals(id));
    }
}
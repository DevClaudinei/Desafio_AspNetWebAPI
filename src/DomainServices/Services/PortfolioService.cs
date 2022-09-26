using DomainModels;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DomainServices.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public PortfolioService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public long Create(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();

        repository.Add(portfolio);
        _unitOfWork.SaveChanges();

        return portfolio.Id;
    }

    protected TResult GetFieldById<T, TResult>(long id, Expression<Func<T, TResult>> selector)
        where T : class, IEntity
    {
        var repository = _repositoryFactory.Repository<T>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id))
            .Select(selector);

        return repository.SingleOrDefault(query);
    }

    public decimal GetTotalBalance(long id)
    {
        return GetFieldById<Portfolio, decimal>(id, x => x.TotalBalance);
    }

    public Portfolio GetById(long id)
    {
        var repository = _repositoryFactory.Repository<Portfolio>();
        var query = repository.SingleResultQuery()
            .Include(source => source.Include(x => x.PortfolioProducts))
            .AndFilter(x => x.Id.Equals(id));

        return repository.FirstOrDefault(query);
    }

    public IEnumerable<Portfolio> GetAll()
    {
        var repository = _repositoryFactory.Repository<Portfolio>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    private bool Exists(long id)
    {
        var repository = _repositoryFactory.Repository<Portfolio>();
        return repository.Any(x => x.Id.Equals(id));
    }

    public bool UpdateBalanceAfterPurchase(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        Exists(portfolio.Id);

        repository.Update(portfolio);
        _unitOfWork.SaveChanges();

        return true;
    }

    public void Delete(long id)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var totalBalance = GetTotalBalance(id);
        if (totalBalance > 0) throw new BadRequestException($"Não é possível deletar carteira, pois ela ainda possui saldo.");

        repository.Remove(x => x.Id.Equals(id));
    }
}
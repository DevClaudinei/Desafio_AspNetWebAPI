using Application.Models.Order;
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
            .Include(x => x.Include(x => x.Products))
            .AndFilter(x => x.Id.Equals(id));

        return repository.FirstOrDefault(query);
    }

    public IEnumerable<Portfolio> GetAll()
    {
        var repository = _repositoryFactory.Repository<Portfolio>();
        var query = repository.MultipleResultQuery()
            .Include(x => x.Include(x => x.Products));

        return repository.Search(query);
    
    }

    public bool Update(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        //portfolio.Products.Clear();

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

    public void AddProduct(Portfolio portfolio, Product product)
    {
        portfolio.Products.Clear();
        portfolio.Products.Add(product);
        Update(portfolio);
    }

    public void RemoveProduct(Portfolio portfolio, Product product)
    {
        portfolio.Products.Remove(product);
        Update(portfolio);
    }

    public void UpdateWithdraw(Order order, long productId, Portfolio portfolio)
    {
        
    }
}
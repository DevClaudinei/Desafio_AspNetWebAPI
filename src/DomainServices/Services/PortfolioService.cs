using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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
        var portfolioExists = GetById(portfolio.Id);

        if (portfolioExists is not null) throw new BadRequestException($"Portfolio with Id: {portfolio.Id} already exists.");

        repository.Add(portfolio);
        _unitOfWork.SaveChanges();

        return portfolio.Id;
    }

    public decimal GetTotalBalance(long portfolioId)
    {
        var portfolioFound = GetById(portfolioId);

        if (portfolioFound is null) throw new NotFoundException($"Portfolio with Id: {portfolioId} not found.");

        return portfolioFound.TotalBalance;
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
        var query = repository.MultipleResultQuery()
            .Include(source => source.Include(x => x.PortfolioProducts));

        return repository.Search(query);
    }

    public void Update(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var portfolioToUpdate = Exists(portfolio);

        repository.Update(portfolioToUpdate);
        _unitOfWork.SaveChanges();
    }

    private Portfolio Exists(Portfolio portfolio)
    {
        var updatedPortfolio = GetById(portfolio.Id);

        if (updatedPortfolio is null) throw new NotFoundException($"Portfolio with Id: {portfolio.Id} not found.");

        portfolio.CreatedAt = updatedPortfolio.CreatedAt;
        return portfolio;
    }

    public bool UpdateBalanceAfterPurchase(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var portfolioToUpdate = Exists(portfolio);

        repository.Update(portfolioToUpdate);
        _unitOfWork.SaveChanges();

        return true;
    }

    public void Delete(long id)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        GetTotalBalance(id);

        repository.Remove(x => x.Id.Equals(id));
    }
}
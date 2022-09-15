using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DomainServices.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public PortfolioService(IUnitOfWork unitOfWork, IRepositoryFactory repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public long CreatePortfolio(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var portfolioExists = GetPortfolioById(portfolio.Id);

        if (portfolioExists is not null) throw new CustomerException($"Portfolio com o ID: {portfolio.Id} já existe");

        repository.Add(portfolio);
        _unitOfWork.SaveChanges();

        return portfolio.Id;
    }

    public decimal GetTotalBalance(long portfolioId)
    {
        var portfolioFound = GetPortfolioById(portfolioId);

        if (portfolioFound is null) throw new CustomerException($"Portfolio com ID: {portfolioId} não localizado.");

        return portfolioFound.TotalBalance;
    }

    public Portfolio GetPortfolioById(long id)
    {
        var repository = _repositoryFactory.Repository<Portfolio>();
        var query = repository.SingleResultQuery()
            .Include(source => source.Include(x => x.PortfolioProducts))
            .AndFilter(x => x.Id.Equals(id));

        return repository.FirstOrDefault(query);
    }

    public IEnumerable<Portfolio> GetAllPortfolios()
    {
        var repository = _repositoryFactory.Repository<Portfolio>();
        var query = repository.MultipleResultQuery()
            .Include(source => source.Include(x => x.PortfolioProducts));

        return repository.Search(query);
    }

    public void Update(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var portfolioToUpdate = VerifyPortfolioAlreadyExists(portfolio);

        repository.Update(portfolioToUpdate);
        _unitOfWork.SaveChanges();
    }

    private Portfolio VerifyPortfolioAlreadyExists(Portfolio portfolio)
    {
        var updatedPortfolio = GetPortfolioById(portfolio.Id);

        if (updatedPortfolio is null) throw new CustomerException($"Portfolio com o ID: {portfolio.Id} não encontrado.");

        portfolio.CreatedAt = updatedPortfolio.CreatedAt;
        return portfolio;
    }

    public bool UpdateBalanceAfterPurchase(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var portfolioToUpdate = VerifyPortfolioAlreadyExists(portfolio);

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

using DomainModels.Entities;
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

    public PortfolioService(
        IUnitOfWork unitOfWork,
        IRepositoryFactory repositoryFactory
    )
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public (bool isValid, string message) CreatePortfolio(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var portfolioExists = GetPortfolioById(portfolio.Id);

        if (portfolioExists is not null) return (false, $"Portfolio com o ID: {portfolio.Id} já existe");

        repository.Add(portfolio);
        _unitOfWork.SaveChanges();

        return (true, portfolio.Id.ToString());
    }

    public (bool isValid, string message) GetTotalBalance(Guid portfolioId)
    {
        var portfolioFound = GetPortfolioById(portfolioId);

        if (portfolioFound is null) return (false, $"Portfolio com ID: {portfolioId} não localizado.");
        if (portfolioFound.TotalBalance == 0) return (true, $"{portfolioFound.TotalBalance}");

        return (true, $"{portfolioFound.TotalBalance}");
    }

    public Portfolio GetPortfolioById(Guid id)
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

    public (bool isValid, string message) Update(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var updatedPortfolio = GetPortfolioById(portfolio.Id);
        
        portfolio.CreatedAt = updatedPortfolio.CreatedAt;
        if (updatedPortfolio is null) return (false, $"Portfolio com o ID: {portfolio.Id} não encontrado.");

        repository.Update(portfolio);
        _unitOfWork.SaveChanges();

        return (true, portfolio.Id.ToString());
    }

    public bool Delete(Guid id)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var checkAccountBalance = GetTotalBalance(id);

        return repository.Remove(x => x.Id.Equals(id)) > 0;
    }
}

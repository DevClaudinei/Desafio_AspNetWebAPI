using DomainModels.Entities;
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
    public (bool isValid, string message) CreatePortfolio(Portfolio portfolio)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        var checkAccountBalance = ChecksIfTheCustomerBankInfoHasBalance(portfolio);

        if (!checkAccountBalance.exists) return (false, checkAccountBalance.errorMessage);

        repository.Add(portfolio);
        _unitOfWork.SaveChanges();

        return (true, portfolio.Id.ToString());
    }

    private (bool exists, string errorMessage) ChecksIfTheCustomerBankInfoHasBalance(Portfolio portfolio)
    {
        var messageTemplate = "The {0} cannot be added to the portfolio. Current account balance is {1}.";
        var customerRepository = _unitOfWork.Repository<Customer>();
        var query = customerRepository.MultipleResultQuery()
            .Include(source => source.Include(x => x.CustomerBankInfo));
        var queryBankInfo = customerRepository.SingleOrDefault(query);

        foreach (var item in portfolio.Products)
        {
            if (item.NetValue > queryBankInfo.CustomerBankInfo.AccountBalance)
            {
                return (false, string.Format(messageTemplate, "Products", queryBankInfo.CustomerBankInfo.AccountBalance));
            }

            portfolio.TotalBalance += item.NetValue;
            queryBankInfo.CustomerBankInfo.AccountBalance -= item.NetValue;
        }
        customerRepository.Update(queryBankInfo);
        _unitOfWork.SaveChanges();

        return (true, portfolio.Id.ToString());
    }

    public decimal GetTotalBalance(Guid portfolioId)
    {
        var portfolioFound = PortfolioById(portfolioId);
        decimal totalBalance = 0;
        foreach (var item in portfolioFound.Products)
        {
            totalBalance += item.NetValue;
        }
        return totalBalance;
    }

    public Portfolio PortfolioById(Guid id)
    {
        var repository = _repositoryFactory.Repository<Portfolio>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.FirstOrDefault(query);
    }

    public IEnumerable<Portfolio> GetAllPortfolios()
    {
        var repository = _repositoryFactory.Repository<Portfolio>();
        var query = repository.MultipleResultQuery()
            .Include(source => source.Include(x => x.Products));

        return repository.Search(query);
    }

    public bool Delete(Guid id)
    {
        var repository = _unitOfWork.Repository<Portfolio>();
        return repository.Remove(x => x.Id.Equals(id)) > 0;
    }
}

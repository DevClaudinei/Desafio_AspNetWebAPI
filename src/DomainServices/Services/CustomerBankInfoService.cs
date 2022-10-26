using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;

namespace DomainServices.Services;

public class CustomerBankInfoService : BaseService, ICustomerBankInfoService
{
    public CustomerBankInfoService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory) { }

    public long GetIdByCustomerId(long id)
    {
        var repository = RepositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery()
            .Select(x => x.Id)
            .AndFilter(x => x.CustomerId.Equals(id));

        return repository.SingleOrDefault(query);
    }

    public IEnumerable<CustomerBankInfo> GetAll()
    {
        var repository = RepositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public CustomerBankInfo GetByAccount(string account)
    {
        var repository = RepositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Account.Equals(account));

        return repository.SingleOrDefault(query);
    }

    public CustomerBankInfo GetById(long id)
    {
        var repository = RepositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(query);
    }

    public decimal GetAccountBalanceById(long id)
    {
        return GetFieldById<CustomerBankInfo, decimal>(id, x => x.AccountBalance);
    }

    public void Deposit(long id, decimal amount)
    {
        var repository = UnitOfWork.Repository<CustomerBankInfo>();
        var customerBankInfo = GetById(id);

        if (customerBankInfo is null) 
            throw new NotFoundException($"CustomerBankInfo for id: {id} not found.");

        customerBankInfo.AccountBalance += amount;

        repository.Update(customerBankInfo, x => x.AccountBalance);
        UnitOfWork.SaveChanges();
    }

    public void Withdraw(long id, decimal amount)
    {
        var repository = UnitOfWork.Repository<CustomerBankInfo>();
        var customerBankInfo = GetById(id);

        if (customerBankInfo is null)
            throw new NotFoundException($"CustomerBankInfo for id: {id} not found.");

        customerBankInfo.AccountBalance -= amount;

        repository.Update(customerBankInfo, x => x.AccountBalance);
        UnitOfWork.SaveChanges();
    }

    public void Create(long customerId)
    {
        var repository = UnitOfWork.Repository<CustomerBankInfo>();
        var customerBankInfo = new CustomerBankInfo(customerId);

        repository.Add(customerBankInfo);
        UnitOfWork.SaveChanges();
    }

    public CustomerBankInfo GetByCustomerId(long id)
    {
        var repository = RepositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.CustomerId.Equals(id));

        return repository.SingleOrDefault(query);
    }
}
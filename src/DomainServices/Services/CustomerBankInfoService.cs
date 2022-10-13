using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;

namespace DomainServices.Services;

public class CustomerBankInfoService : BaseService, ICustomerBankInfoService
{
    private readonly IRepository<CustomerBankInfo> _customerBankInfoService;
    
    public CustomerBankInfoService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory)
    {
        _customerBankInfoService = RepositoryFactory.Repository<CustomerBankInfo>();
    }

    public long GetIdByCustomerId(long id)
    {
        var query = _customerBankInfoService.SingleResultQuery()
            .Select(x => x.Id)
            .AndFilter(x => x.CustomerId.Equals(id));

        return _customerBankInfoService.SingleOrDefault(query);
    }

    public IEnumerable<CustomerBankInfo> GetAll()
    {
        var query = _customerBankInfoService.MultipleResultQuery();

        return _customerBankInfoService.Search(query);
    }

    public CustomerBankInfo GetByAccount(string account)
    {
        var query = _customerBankInfoService.SingleResultQuery()
            .AndFilter(x => x.Account.Equals(account));

        return _customerBankInfoService.SingleOrDefault(query);
    }

    public CustomerBankInfo GetById(long id)
    {
        var query = _customerBankInfoService.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return _customerBankInfoService.SingleOrDefault(query);
    }

    public decimal GetAccountBalanceById(long id)
    {
        return GetFieldById<CustomerBankInfo, decimal>(id, x => x.AccountBalance);
    }

    public void Deposit(long id, decimal amount)
    {
        var repository = UnitOfWork.Repository<CustomerBankInfo>();
        var customerBankInfo = GetById(id);
        customerBankInfo.AccountBalance += amount;

        repository.Update(customerBankInfo, x => x.AccountBalance);
        UnitOfWork.SaveChanges();
    }

    public void Withdraw(long id, decimal amount)
    {
        var repository = UnitOfWork.Repository<CustomerBankInfo>();
        var customerBankInfo = GetById(id);
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
}
using DomainModels;
using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DomainServices.Services;

public class CustomerBankInfoService : ICustomerBankInfoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public CustomerBankInfoService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public TResult GetFieldById<T, TResult>(long id, Expression<Func<T, TResult>> selector)
        where T : class, IEntity
    {
        var repository = _repositoryFactory.Repository<T>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id))
            .Select(selector);

        return repository.SingleOrDefault(query);
    }

    public long GetCustomerBankInfoId(long id)
    {
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery()
            .Select(x => x.Id)
            .AndFilter(x => x.CustomerId.Equals(id));

        return repository.SingleOrDefault(query);
    }

    public IEnumerable<CustomerBankInfo> GetAll()
    {
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public CustomerBankInfo GetByAccount(string account)
    {
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Account.Equals(account));

        return repository.SingleOrDefault(query);
    }

    public CustomerBankInfo GetById(long id)
    {
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();
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
        var repository = _unitOfWork.Repository<CustomerBankInfo>();
        var customerBankInfo = GetById(id);
        customerBankInfo.AccountBalance += amount;

        repository.Update(customerBankInfo, x => x.AccountBalance);
        _unitOfWork.SaveChanges();
    }

    public void Withdraw(long id, decimal amount)
    {
        var repository = _unitOfWork.Repository<CustomerBankInfo>();
        var customerBankInfo = GetById(id);
        customerBankInfo.AccountBalance -= amount;

        repository.Update(customerBankInfo, x => x.AccountBalance);
        _unitOfWork.SaveChanges();
    }

    public void Create(long customerId)
    {
        var repository = _unitOfWork.Repository<CustomerBankInfo>();
        var customerBankInfo = GenerateAccountNumber(customerId);

        repository.Add(customerBankInfo);
        _unitOfWork.SaveChanges();
    }
    
    private CustomerBankInfo GenerateAccountNumber(long customerId)
    {
        var numberAccountValid = false;
        var listaContas = GetAll();
        var customerBankInfo = new CustomerBankInfo(customerId);
        
        if (listaContas.Count() == 0) numberAccountValid = true;

        while (numberAccountValid is false)
        {
            foreach (var conta in listaContas)
            {
                if (conta.Account == customerBankInfo.Account) customerBankInfo.Account = new Random().Next(1, 100).ToString();
            }

            numberAccountValid = true;
        }
        return customerBankInfo;
    }
}
using DomainModels;
using DomainModels.Entities;
using DomainServices.Exceptions;
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

    public long RetornaIdDeContaAtravesDeCustomerID(long id)
    {
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery().Select(x => x.Id);

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

    private CustomerBankInfo CheckIfWithdrawalIsValid(CustomerBankInfo updatedCustomerBankInfo, CustomerBankInfo customerBankInfo)
    {
        if (customerBankInfo.AccountBalance < 0) throw new BadRequestException($"Unable to make negative withdrawals.");

        customerBankInfo.AccountBalance = updatedCustomerBankInfo.AccountBalance - customerBankInfo.AccountBalance;

        if (updatedCustomerBankInfo.AccountBalance <= 0) throw new BadRequestException($"Requested balance cannot be withdraw.");

        return customerBankInfo;
    }

    private CustomerBankInfo Exists(CustomerBankInfo customerBankInfo)
    {
        var updatedCustomerBankInfo = GetByAccount(customerBankInfo.Account);

        if (updatedCustomerBankInfo is null)
            throw new NotFoundException($"CustomerBankInfo for the Account: {customerBankInfo.Account} not found.");

        if (updatedCustomerBankInfo.CustomerId != customerBankInfo.CustomerId)
            throw new NotFoundException($"Customer for the Id: {customerBankInfo.CustomerId} not found.");

        if (updatedCustomerBankInfo.Id != customerBankInfo.Id)
            throw new NotFoundException($"CustomerBankInfo for the Id: {customerBankInfo.Id} not found.");

        customerBankInfo.CreatedAt = updatedCustomerBankInfo.CreatedAt;

        return updatedCustomerBankInfo;
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
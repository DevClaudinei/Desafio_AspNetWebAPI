using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public IEnumerable<CustomerBankInfo> Get()
    {
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public CustomerBankInfo GetCustomerBankInfoByAccount(string account)
    {
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Account.Equals(account));

        return repository.SingleOrDefault(query);
    }

    public CustomerBankInfo GetCustomerBankInfoById(long id)
    {
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(query);
    }

    public void DepositMoney(long id, CustomerBankInfo customerBankInfo)
    {
        var repository = _unitOfWork.Repository<CustomerBankInfo>();
        customerBankInfo.Id = id;
        var updatedCustomerBankInfo = VerifyCustomerBankInfoHasBalance(customerBankInfo);
        customerBankInfo.AccountBalance = updatedCustomerBankInfo.AccountBalance + customerBankInfo.AccountBalance;

        repository.Update(customerBankInfo);
        _unitOfWork.SaveChanges();
    }

    private CustomerBankInfo VerifyCustomerBankInfoHasBalance(CustomerBankInfo customerBankInfo)
    {
        var updatedCustomerBankInfo = VerifyCustomerBankInfoExists(customerBankInfo);

        if (customerBankInfo.AccountBalance < 0) 
            throw new GenericBalancesException($"CustomerBankInfo cannot update balance for negative amounts.");

        return updatedCustomerBankInfo;
    }

    public void WithdrawMoney(long id, CustomerBankInfo customerBankInfo)
    {
        var repository = _unitOfWork.Repository<CustomerBankInfo>();
        customerBankInfo.Id = id;
        var updatedCustomerBankInfo = VerifyCustomerBankInfoExists(customerBankInfo);
        var customerBankInfoToUpdate = CheckIfWithdrawalIsValid(updatedCustomerBankInfo, customerBankInfo);

        repository.Update(customerBankInfoToUpdate);
        _unitOfWork.SaveChanges();
    }

    private CustomerBankInfo CheckIfWithdrawalIsValid(CustomerBankInfo updatedCustomerBankInfo, CustomerBankInfo customerBankInfo)
    {
        if (customerBankInfo.AccountBalance < 0) throw new GenericBalancesException($"Unable to make negative withdrawals.");

        customerBankInfo.AccountBalance = updatedCustomerBankInfo.AccountBalance - customerBankInfo.AccountBalance;

        if (updatedCustomerBankInfo.AccountBalance <= 0) throw new GenericBalancesException($"Requested balance cannot be redeemed.");

        return customerBankInfo;
    }

    private CustomerBankInfo VerifyCustomerBankInfoExists(CustomerBankInfo customerBankInfo)
    {
        var updatedCustomerBankInfo = GetCustomerBankInfoByAccount(customerBankInfo.Account);

        if (updatedCustomerBankInfo is null)
            throw new GenericNotFoundException($"CustomerBankInfo for the Account: {customerBankInfo.Account} not found.");

        if (updatedCustomerBankInfo.CustomerId != customerBankInfo.CustomerId)
            throw new GenericNotFoundException($"Customer for the Id: {customerBankInfo.CustomerId} not found.");

        if (updatedCustomerBankInfo.Id != customerBankInfo.Id)
            throw new GenericNotFoundException($"CustomerBankInfo for the Id: {customerBankInfo.Id} not found.");

        customerBankInfo.CreatedAt = updatedCustomerBankInfo.CreatedAt;

        return updatedCustomerBankInfo;
    }

    public bool UpdateBalanceAfterPurchase(CustomerBankInfo customerBankInfo)
    {
        var repository = _unitOfWork.Repository<CustomerBankInfo>();
        var customerBankInfoToUpdate = GetCustomerBankInfoById(customerBankInfo.Id);
        customerBankInfo.CreatedAt = customerBankInfoToUpdate.CreatedAt;

        repository.Update(customerBankInfo);
        _unitOfWork.SaveChanges();

        return true;
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
        var listaContas = Get();
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
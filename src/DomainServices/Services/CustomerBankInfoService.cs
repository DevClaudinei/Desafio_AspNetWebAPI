using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;

namespace DomainServices.Services;

public class CustomerBankInfoService : ICustomerBankInfoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public CustomerBankInfoService(IUnitOfWork unitOfWork, IRepositoryFactory repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public (bool isValid, string message) CreateCustomerBankInfo(CustomerBankInfo customerBankInfo, long customerId)
    {
        customerBankInfo.CustomerId = customerId;
        var customerBankInfoAlreadyExists = VerifyCustomerBankInfoAlreadyExists(customerBankInfo);

        if (customerBankInfoAlreadyExists.exists) return (false, customerBankInfoAlreadyExists.errorMessage);

        var repository = _unitOfWork.Repository<CustomerBankInfo>();

        repository.Add(customerBankInfo);
        _unitOfWork.SaveChanges();

        return (true, customerBankInfo.Id.ToString());
    }

    private (bool exists, string errorMessage) VerifyCustomerBankInfoAlreadyExists(CustomerBankInfo customerBankInfo)
    {
        var messageTemplate = "The {0}: {1} is already linked to a|an {2}";
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();

        if (repository.Any(x => x.Account.Equals(customerBankInfo.Account)))
        {
            return (true, string.Format(messageTemplate, "Account", customerBankInfo.Account, "Customer"));
        }

        if (repository.Any(x => x.Customer.Id.Equals(customerBankInfo.CustomerId)))
        {
            return (true, string.Format(messageTemplate, "CustomerId", customerBankInfo.CustomerId, "Account"));
        }

        return default;
    }

    public IEnumerable<CustomerBankInfo> GetAllCustomerBankInfo()
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

    public (bool isValid, string message) DepositMoney(CustomerBankInfo customerBankInfo)
    {
        var customerBankInfoToDesposityMoney = VerifyCustomerBankInfoExists(customerBankInfo);
        var repository = _unitOfWork.Repository<CustomerBankInfo>();

        if (!customerBankInfoToDesposityMoney.exists) return (false, customerBankInfoToDesposityMoney.errorMessage);

        repository.Update(customerBankInfo);
        _unitOfWork.SaveChanges();

        return (true, customerBankInfo.Id.ToString());
    }

    private (bool exists, string errorMessage) VerifyCustomerBankInfoExists(CustomerBankInfo customerBankInfo)
    {
        var messageTemplate = "{0} para o {1}: {2} não localizado.";
        var updatedCustomerBankInfo = GetCustomerBankInfoByAccount(customerBankInfo.Account);

        if (updatedCustomerBankInfo is null)
            return (false, $"CustomerBankInfo not found by Account: {customerBankInfo.Account}.");

        if (updatedCustomerBankInfo.CustomerId != customerBankInfo.CustomerId)
            return (true, string.Format(messageTemplate,"Customer", "Id", customerBankInfo.CustomerId));

        if (updatedCustomerBankInfo.Id != customerBankInfo.Id)
            return (true, string.Format(messageTemplate, "CustomerBankInfo", "Id", customerBankInfo.Id));

        customerBankInfo.CreatedAt = updatedCustomerBankInfo.CreatedAt;

        if (customerBankInfo.AccountBalance < 0) return (false, $"CustomerBankInfo cannot update balance for negative amounts.");

        customerBankInfo.AccountBalance = updatedCustomerBankInfo.AccountBalance + customerBankInfo.AccountBalance;

        return (true, "");
    }

    public (bool isValid, string message) WithdrawMoney(CustomerBankInfo customerBankInfo)
    {
        var updatedCustomerBankInfo = GetCustomerBankInfoById(customerBankInfo.Id);
        if (updatedCustomerBankInfo is null) return (false, $"Conta para o Id: {customerBankInfo.Id} não localizada.");

        customerBankInfo.CreatedAt = updatedCustomerBankInfo.CreatedAt;
        var repository = _unitOfWork.Repository<CustomerBankInfo>();

        if (updatedCustomerBankInfo.AccountBalance <= 0) return (false, $"Saldo solicitado não pode ser resgatado.");

        customerBankInfo.AccountBalance = updatedCustomerBankInfo.AccountBalance - customerBankInfo.AccountBalance;

        if (updatedCustomerBankInfo.AccountBalance < 0) return (false, $"Saldo solicitado não pode ser resgatado.");

        repository.Update(customerBankInfo);
        _unitOfWork.SaveChanges();

        return (true, "");
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

    public (bool isValid, string message) Delete(long id)
    {
        var messageTemplate = "The {0}: {1} cannot be closed as it still has a balance";
        var checkIfAccountHasBalance = GetCustomerBankInfoById(id);
        if (checkIfAccountHasBalance.AccountBalance > 0) 
            return (false, string.Format(messageTemplate, "Account", checkIfAccountHasBalance.Account));

        var repository = _unitOfWork.Repository<CustomerBankInfo>();

        return (repository.Remove(x => x.Id.Equals(id)) > 0, "");
    }
}

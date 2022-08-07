using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;

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

    public (bool isValid, string message) CreateCustomerBankInfo(CustomerBankInfo customerBankInfo)
    {
        var customerBankInfoAlreadyExists = VerifyCustomerBankInfoAlreadyExists(customerBankInfo);

        if (customerBankInfoAlreadyExists.exists) return (false, customerBankInfoAlreadyExists.errorMessage);

       var repository = _unitOfWork.Repository<CustomerBankInfo>();

        repository.Add(customerBankInfo);
        _unitOfWork.SaveChanges();

        return (true, customerBankInfo.Id.ToString());
    }

    private (bool exists, string errorMessage) VerifyCustomerBankInfoAlreadyExists(CustomerBankInfo customerBankInfo)
    {
        var messageTemplate = "CustomerBankInfo already exists for {0}";
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();

        if (repository.Any(x => x.Account.Equals(customerBankInfo.Account)))
        {
            return (true, string.Format(messageTemplate, "Email", customerBankInfo.Account));
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

    public CustomerBankInfo GetCustomerBankInfoById(Guid id)
    {
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(query);
    }

    public (bool isValid, string message) UpdateCustomerBankInfo(CustomerBankInfo customerBankInfo)
    {
        var repository = _unitOfWork.Repository<CustomerBankInfo>();

        repository.Update(customerBankInfo);
        _unitOfWork.SaveChanges();

        return (true, customerBankInfo.Id.ToString());
    }
    public bool Delete(Guid id)
    {
        var repository = _unitOfWork.Repository<CustomerBankInfo>();

        return repository.Remove(x => x.Id.Equals(id)) > 0;
    }
}

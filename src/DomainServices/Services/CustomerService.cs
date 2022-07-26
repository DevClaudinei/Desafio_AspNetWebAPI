using System;
using System.Collections.Generic;
using DomainModels;
using DomainServices.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;

namespace DomainServices;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;
    
    public CustomerService(IUnitOfWork unitOfWork, IRepositoryFactory repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public (bool isValid, string message) CreateCustomer(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);
        
        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        var repository = _unitOfWork.Repository<Customer>();

        if (repository is null) return (false, customerAlreadyExists.errorMessage);

        repository.Add(customer);
        _unitOfWork.SaveChanges();

        return (true, customer.Id.ToString());
    }

    private (bool exists, string errorMessage) VerifyCustomerAlreadyExists(Customer customer)
    {
        var messageTemplate = "Customer already exists for {0}: {1}";
        var repository = _repositoryFactory.Repository<Customer>();



        if (repository.Any(x => x.Equals(customer.Email)))
        {
            return (true, string.Format(messageTemplate, "Email", customer.Email));
        }

        if (repository.Any(x => x.Equals(customer.Cpf)))
        {
            return (true, string.Format(messageTemplate, "Cpf", customer.Cpf));
        }

        return default;
    }

    public IEnumerable<Customer> GetAll()
    {
        var repository = _repositoryFactory.Repository<Customer>();
        var query = repository.MultipleResultQuery();
        return repository.Search(query); 
    }

    public Customer GetById(Guid id)
    {
        var repository = _repositoryFactory.Repository<Customer>();
        var customerFound = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));
        return repository.SingleOrDefault(customerFound);
    }

    public Customer GetByFullName(string fullName)
    {
        var repository = _repositoryFactory.Repository<Customer>();
        var customerFound = repository.SingleResultQuery()
            .AndFilter(x => x.FullName.Equals(fullName));
        return repository.SingleOrDefault(customerFound);
    }

    public (bool isValid, string message) Update(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);

        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        var updatedCustomer = GetById(customer.Id);
        customer.CreatedAt = updatedCustomer.CreatedAt;

        if (updatedCustomer is null) return (false, $"Cliente não encontrado para o Id: {customer.Id}.");

        var repository = _unitOfWork.Repository<Customer>();

        if (repository is null) return (false, customerAlreadyExists.errorMessage);

        return (true, customer.Id.ToString());
    }

    public bool Delete(Guid id)
    {
        var customerFound = GetById(id);
        if (customerFound != null)
        {
            var repository = _unitOfWork.Repository<Customer>();

            if (repository is null) return false;

            return true;
        }
        return false;
    }
}
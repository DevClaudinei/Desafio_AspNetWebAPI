using System;
using System.Collections.Generic;
using DomainModels;
using DomainServices.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;

namespace DomainServices;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;
    
    public CustomerService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public (bool isValid, string message) CreateCustomer(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);
        
        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        var repository = _unitOfWork.Repository<Customer>();

        repository.Add(customer);
        _unitOfWork.SaveChanges();

        return (true, customer.Id.ToString());
    }

    private (bool exists, string errorMessage) VerifyCustomerAlreadyExists(Customer customer)
    {
        var messageTemplate = "Customer already exists for {0}: {1}";
        var repository = _repositoryFactory.Repository<Customer>();

        if (repository.Any(x => x.Email.Equals(customer.Email)))
        {
            return (true, string.Format(messageTemplate, "Email", customer.Email));
        }

        if (repository.Any(x => x.Cpf.Equals(customer.Cpf)))
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

    public IEnumerable<Customer> GetAllByFullName(string fullName)
    {
        var repository = _repositoryFactory.Repository<Customer>();
        var query = repository.MultipleResultQuery()
            .AndFilter(x => x.FullName.Contains(fullName));
        
        return repository.Search(query);
    }

    public (bool isValid, string message) Update(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);

        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        var updatedCustomer = GetById(customer.Id);
        customer.CreatedAt = updatedCustomer.CreatedAt;

        if (updatedCustomer is null) return (false, $"Cliente não encontrado para o Id: {customer.Id}.");

        var repository = _unitOfWork.Repository<Customer>();

        repository.Update(customer);
        _unitOfWork.SaveChanges();

        return (true, customer.Id.ToString());
    }

    public bool Delete(Guid id)
    {
        var repository = _unitOfWork.Repository<Customer>();
        return repository.Remove(x => x.Id.Equals(id)) > 0;
    }
}
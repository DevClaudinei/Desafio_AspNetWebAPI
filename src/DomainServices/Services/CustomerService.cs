using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DomainServices;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public CustomerService(
        IUnitOfWork unitOfWork,
        IRepositoryFactory repositoryFactory
        )
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public long CreateCustomer(Customer customer)
    {
        var repository = _unitOfWork.Repository<Customer>();
        VerifyCustomerAlreadyExists(customer);
        
        repository.Add(customer);
        _unitOfWork.SaveChanges();

        return customer.Id;
    }

    private bool VerifyCustomerAlreadyExists(Customer customer)
    {
        var repository = _repositoryFactory.Repository<Customer>();

        if (repository.Any(x => x.Email.Equals(customer.Email)))
            throw new CustomerException($"Customer already exists for email: {customer.Email}");

        if (repository.Any(x => x.Cpf.Equals(customer.Cpf)))
            throw new CustomerException($"Customer already exists for CPF: {customer.Cpf}");

        return true;
    }

    public IEnumerable<Customer> GetAll()
    {
        var repository = _repositoryFactory.Repository<Customer>();
        var query = repository.MultipleResultQuery().Include(x => x.Include(x => x.CustomerBankInfo));

        return repository.Search(query); 
    }

    public Customer GetById(long id)
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

    private Customer CustomerAlreadyExists(long id)
    {
        var updatedCustomer = GetById(id);
        if (updatedCustomer is null) throw new CustomerException($"Cliente não encontrado para o Id: {id}.");

        return updatedCustomer;
    }

    public void Update(long id, Customer customer)
    {
        var repository = _unitOfWork.Repository<Customer>();
        VerifyCustomerAlreadyExists(customer);
        var updatedCustomer = CustomerAlreadyExists(id);
        
        customer.Id = id;
        customer.CreatedAt = updatedCustomer.CreatedAt;

        repository.Update(customer);
        _unitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var repository = _unitOfWork.Repository<Customer>();
        CustomerAlreadyExists(id);

        repository.Remove(x => x.Id.Equals(id));
    }
}
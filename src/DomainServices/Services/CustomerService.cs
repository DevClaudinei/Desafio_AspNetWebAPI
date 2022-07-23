using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels;
using DomainServices.GenericRepositories.Interface;
using DomainServices.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;

namespace DomainServices;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Customer> _genericRepository;

    public CustomerService(ApplicationDbContext context, IUnitOfWork unitOfWork, IGenericRepository<Customer> genericRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _genericRepository = genericRepository; 
    }

    public (bool isValid, string message) CreateCustomer(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);
        
        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        var customerToCreate = _genericRepository.Insert(customer);

        if (customerToCreate is false) return (false, customerAlreadyExists.errorMessage);

        return (true, customer.Id.ToString());
    }

    private (bool exists, string errorMessage) VerifyCustomerAlreadyExists(Customer customer)
    {
        var messageTemplate = "Customer already exists for {0}: {1}";
        var repository = _unitOfWork.Repository<Customer>();
        

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
        return _genericRepository.GetAll();
    }

    public Customer GetById(Guid id)
    {
        return _genericRepository.GetById(id);
    }

    public Customer GetByFullName(string fullName)
    {
        var x = _genericRepository.GetByName(fullName).Cast<Customer>();
        return (Customer)x;
    }

    public (bool isValid, string message) Update(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);

        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        var updatedCustomer = GetById(customer.Id);
        customer.CreatedAt = updatedCustomer.CreatedAt;

        if (updatedCustomer is null) return (false, $"Cliente não encontrado para o Id: {customer.Id}.");

        var customerToUpdate = _genericRepository.Update(customer);

        if (customerToUpdate is false) return (false, customerAlreadyExists.errorMessage);

        return (true, customer.Id.ToString());
    }

    public bool Delete(Guid id)
    {
        var customerFound = GetById(id);
        if (customerFound != null)
        {
            var customerToDelete = _genericRepository.Delete(id);

            if (customerToDelete is false) return false;

            return true;
        }
        return false;
    }
}
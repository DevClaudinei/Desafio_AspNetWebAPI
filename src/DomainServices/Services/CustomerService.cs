using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels;
using DomainServices.Services;

namespace DomainServices;

public class CustomerService : ICustomerService
{
    private IList<Customer> _customers = new List<Customer>();

    public (bool isValid, string message) CreateCustomer(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);
            ;

        customer.Id = Guid.NewGuid();
        _customers.Add(customer);

        return (true, customer.Id.ToString());
    }

    public IList<Customer> GetCustomers()
    {
        return _customers;
    }

    private (bool exists, string errorMessage) VerifyCustomerAlreadyExists(Customer customer)
    {
        var messageTemplate = "Customer already exists for {0}: {1}";
        if (_customers.Any(x => x.Email.Equals(customer.Email)))
        {
            return (true, string.Format(messageTemplate, "Email", customer.Email));
        }

        if (_customers.Any(x => x.Cpf.Equals(customer.Cpf)))
        {
            return (true, string.Format(messageTemplate, "Cpf", customer.Cpf));
        }

        return default;
    }

    public Customer GetById(Guid Id)
    {
        var comparedCustomerByIds = _customers
            .FirstOrDefault(a => a.Id.Equals(Id));
        return comparedCustomerByIds;
    }

    public Customer GetByFullName(string fullName)
    {
        var comparedCustomerByNames = _customers
            .FirstOrDefault(a => a.FullName.Contains(fullName));
        return comparedCustomerByNames;
    }

    public (bool isValid, string message) Update(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);

        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        var customerExists = _customers.FirstOrDefault(x => x.Id.Equals(customer.Id));
        var indexCustomer = _customers.IndexOf(customerExists);
        
        if (customerExists is null) return (false, $"Cliente não encontrado para o ID: {customer.Id}.");

        _customers[indexCustomer] = customer;

        return (true, customer.Id.ToString());
    }

    public bool Delete(Guid id)
    {
        var customerFound = GetById(id);
        if (customerFound != null)
        {
            _customers.Remove(customerFound);
            return true;
        }
        return false;
    }
}
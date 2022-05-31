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
        if (CustomerForEmailAlreadyExists(customer.Email)) return (false, $"Cliente para o email: {customer.Email} já existe.");
        if (CustomerForCpfAlreadyExists(customer.Cpf)) return (false, $"Cliente para o cpf: {customer.Cpf} já existe.");

        _customers.Add(customer);
        
        return (true, null);
    }

    public IList<Customer> GetCustomers()
    {
        return _customers;
    }

    public bool CustomerForCpfAlreadyExists(string cpf)
    {
        return _customers.Any(x => x.Cpf.Equals(cpf));
    }

    public bool CustomerForEmailAlreadyExists(string email)
    {
        return _customers.Any(x => x.Email.Equals(email));
    }

    public Customer GetCustomerById(Guid Id)
    {
        var ComparedCustomerByIds = _customers
            .FirstOrDefault(a => a.Id.Equals(Id));
        return ComparedCustomerByIds;
    }

    public Customer GetCustomerByName(string fullName)
    {
        var ComparedCustomerByNames = _customers
            .FirstOrDefault(a => a.FullName.Contains(fullName));
        return ComparedCustomerByNames;
    }

    public Customer UpdateCustomer(Customer customer)
    {

        var CustomerFound = GetCustomerById(customer.Id);
        if (CustomerFound is null) return null;

        CustomerFound.FullName = customer.FullName;
        CustomerFound.Email = customer.Email;
        CustomerFound.EmailConfirmation = customer.EmailConfirmation;
        CustomerFound.Cpf = customer.Cpf;
        CustomerFound.Cellphone = customer.Cellphone;
        CustomerFound.Birthdate = customer.Birthdate;
        CustomerFound.EmailSms = customer.EmailSms;
        CustomerFound.Whatsapp = customer.Whatsapp;
        CustomerFound.Country = customer.Country;
        CustomerFound.City = customer.City;
        CustomerFound.PostalCode = customer.PostalCode;
        CustomerFound.Address = customer.Address;
        CustomerFound.Number = customer.Number;
        CustomerFound.ModifiedAt = DateTime.UtcNow;
        
        return CustomerFound;
    }

    public bool DeleteCustomer(Guid id)
    {
        var CustomerFound = GetCustomerById(id);
        if (CustomerFound != null)
        {
            _customers.Remove(CustomerFound);
            return true;
        }
        return false;
    }
}
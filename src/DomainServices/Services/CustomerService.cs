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
        customer.Id = Guid.NewGuid();
        _customers.Add(customer);
        
        return (true, customer.Id.ToString());
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
        var comparedCustomerByIds = _customers
            .FirstOrDefault(a => a.Id.Equals(Id));
        return comparedCustomerByIds;
    }

    public Customer GetCustomerByName(string fullName)
    {
        var comparedCustomerByNames = _customers
            .FirstOrDefault(a => a.FullName.Contains(fullName));
        return comparedCustomerByNames;
    }

    public Customer UpdateCustomer(Customer customer)
    {

        var customerFound = GetCustomerById(customer.Id);
        if (customerFound is null) return null;

        customerFound.FullName = customer.FullName;
        customerFound.Email = customer.Email;
        customerFound.EmailConfirmation = customer.EmailConfirmation;
        customerFound.Cpf = customer.Cpf;
        customerFound.Cellphone = customer.Cellphone;
        customerFound.Birthdate = customer.Birthdate;
        customerFound.EmailSms = customer.EmailSms;
        customerFound.Whatsapp = customer.Whatsapp;
        customerFound.Country = customer.Country;
        customerFound.City = customer.City;
        customerFound.PostalCode = customer.PostalCode;
        customerFound.Address = customer.Address;
        customerFound.Number = customer.Number;
        customerFound.ModifiedAt = DateTime.UtcNow;
        
        return customerFound;
    }

    public bool DeleteCustomer(Guid id)
    {
        var customerFound = GetCustomerById(id);
        if (customerFound != null)
        {
            _customers.Remove(customerFound);
            return true;
        }
        return false;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels;
using DomainServices.Services;

namespace DomainServices;

public class CustomerService : ICustomerService
{
    private IList<Customer> _customers = new List<Customer>();

    public bool CreateCustomer(Customer customer)
    {
        var ComparedCustomerByEmail = GetCustomerByEmail(customer.Email);
        var ComparedCustomerByCpf = GetCustomerByCpf(customer.Cpf);
        var ValidCustomer = false;

        if (ComparedCustomerByEmail is null && ComparedCustomerByCpf is null)
        {
            customer.Id = Guid.NewGuid();
            customer.CreatedAt = DateTime.Now;
            customer.ModifiedAt = customer.CreatedAt;
            _customers.Add(customer);
            ValidCustomer = true;
        }
        return ValidCustomer;
    }

    public IList<Customer> GetCustomers()
    {
        return _customers;
    }

    public Customer GetCustomerByCpf(string Cpf)
    {
        var ComparedCustomerByCpf = _customers.FirstOrDefault(x => x.Cpf.Equals(Cpf));
        return ComparedCustomerByCpf;
    }

    public Customer GetCustomerByEmail(string Email)
    {
        var ComparedCustomerByEmail = _customers.FirstOrDefault(x => x.Email.Equals(Email));
        return ComparedCustomerByEmail;
    }

    public Customer GetCustomerById(Guid Id)
    {
        var ComparedCustomerByIds = _customers
            .FirstOrDefault(a => a.Id.Equals(Id));
        return ComparedCustomerByIds;
    }

    public Customer GetCustomerByName(string FullName)
    {
        var ComparedCustomerByNames = _customers
            .FirstOrDefault(a => a.FullName.Contains(FullName));
        return ComparedCustomerByNames;
    }

    public Customer UpdateCustomer(Customer customer)
    {
        var CustomerFound = GetCustomerById(customer.Id);
        if (CustomerFound is null) return null;

        CustomerFound.Id = customer.Id;
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
        CustomerFound.ModifiedAt = DateTime.Now;
        return CustomerFound;
    }

    public bool ExcludeCustomer(Guid id)
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
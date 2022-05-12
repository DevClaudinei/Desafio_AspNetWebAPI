using System;
using System.Collections.Generic;
using System.Linq;
using DesafioWarren.WebAPI.Models;

namespace DesafioWarren.WebAPI.Services;

public class CustomerService : ICustomerService
{
    public List<Customer> ListCustomer { get; set; } = new() { };

    public void CreateCustomer(Customer customer)
    {
        customer.Id = Guid.NewGuid();
        ListCustomer.Add(customer);
    }

    public Customer GetCustomerById(Guid Id)
    {
        var ComparedCustomerByIds = ListCustomer
            .FirstOrDefault(a => a.Id.Equals(Id));
        return ComparedCustomerByIds;
    }

    public Customer GetCustomerByName(string FullName)
    {
        var ComparedCustomerByNames = ListCustomer
            .FirstOrDefault(a => a.FullName.Contains(FullName));
        return ComparedCustomerByNames;
    }

    public bool UpdateCustomer(Customer customer)
    {
        var CustomerFound = GetCustomerById(customer.Id);
        try
        {   
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
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool ExcludeCustomer(Guid id)
    {
        var CustomerFound = GetCustomerById(id);

        if (CustomerFound != null)
        {
            ListCustomer.Remove(CustomerFound);
            return true;
        }
        else return false;
    }
}
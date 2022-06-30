using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels;
using DomainServices.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DomainServices;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public (bool isValid, string message) CreateCustomer(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);

        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        _context.Customers.Add(customer);
        _context.SaveChanges();

        return (true, customer.Id.ToString());
    }

    private (bool exists, string errorMessage) VerifyCustomerAlreadyExists(Customer customer)
    {
        var messageTemplate = "Customer already exists for {0}: {1}";
        if (_context.Customers.Any(x => x.Email.Equals(customer.Email)))
        {
            return (true, string.Format(messageTemplate, "Email", customer.Email));
        }

        if (_context.Customers.Any(x => x.Cpf.Equals(customer.Cpf)))
        {
            return (true, string.Format(messageTemplate, "Cpf", customer.Cpf));
        }

        return default;
    }

    public IEnumerable<Customer> GetCustomers()
    {
        return _context.Customers;
    }

    public Customer GetById(Guid Id)
    {
        var comparedCustomerByIds = _context.Customers.AsNoTracking().FirstOrDefault(a => a.Id.Equals(Id));
        return comparedCustomerByIds;
    }

    public Customer GetByFullName(string fullName)
    {
        var comparedCustomerByNames = _context.Customers.FirstOrDefault(a => a.FullName.Contains(fullName));
        return comparedCustomerByNames;
    }

    public (bool isValid, string message) Update(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);

        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        var updatedCustomer = GetById(customer.Id);

        if (updatedCustomer is null) return (false, $"Cliente não encontrado para o Id: {customer.Id}.");

        _context.Update(customer);
        _context.SaveChanges();

        return (true, customer.Id.ToString());
    }

    public bool Delete(Guid id)
    {
        var customerFound = GetById(id);
        if (customerFound != null)
        {
            _context.Customers.Remove(customerFound);
            _context.SaveChanges();
            
            return true;
        }
        return false;
    }
}
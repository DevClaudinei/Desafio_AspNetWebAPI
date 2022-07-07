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
    private readonly DbSet<Customer> entities;

    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
        entities = _context.Set<Customer>();
    }

    public (bool isValid, string message) CreateCustomer(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);

        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        customer.ModifiedAt = DateTime.UtcNow;
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
        var customers = entities.AsEnumerable();
        return customers;
    }

    public Customer GetById(Guid Id)
    {
        return entities.AsNoTracking().SingleOrDefault(a => a.Id.Equals(Id));
    }

    public Customer GetByFullName(string fullName)
    {
        return entities.SingleOrDefault(a => a.FullName.Equals(fullName));
    }

    public (bool isValid, string message) Update(Customer customer)
    {
        var customerAlreadyExists = VerifyCustomerAlreadyExists(customer);

        if (customerAlreadyExists.exists) return (false, customerAlreadyExists.errorMessage);

        var updatedCustomer = GetById(customer.Id);

        if (updatedCustomer is null) return (false, $"Cliente não encontrado para o Id: {customer.Id}.");

        customer.ModifiedAt = DateTime.UtcNow;
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
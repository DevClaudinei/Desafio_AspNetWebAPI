using System;
using System.Collections.Generic;
using DomainModels;

namespace DomainServices.Services;

public interface ICustomerService
{
    (bool isValid, string message) CreateCustomer(Customer customer);

    IList<Customer> GetCustomers();

    bool CustomerForCpfAlreadyExists(string cpf);

    bool CustomerForEmailAlreadyExists(string email);

    Customer GetCustomerById(Guid CustomerId);

    Customer GetCustomerByName(string FullName);

    Customer UpdateCustomer(Customer customer);

    bool ExcludeCustomer(Guid id);
}
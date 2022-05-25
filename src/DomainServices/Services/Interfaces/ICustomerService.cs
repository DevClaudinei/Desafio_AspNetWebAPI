using System;
using System.Collections.Generic;
using DomainModels;

namespace DomainServices.Services;
public interface ICustomerService
{
    bool CreateCustomer(DomainModels.Customer customer);

    IList<Customer> GetCustomers();

    Customer GetCustomerByCpf(string Email);

    Customer GetCustomerByEmail(string Email);

    Customer GetCustomerById(Guid CustomerId);

    Customer GetCustomerByName(string FullName);

    Customer UpdateCustomer(DomainModels.Customer customer);

    bool ExcludeCustomer(Guid id);
}
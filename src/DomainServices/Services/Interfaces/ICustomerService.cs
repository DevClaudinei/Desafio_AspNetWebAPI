using System;
using System.Collections.Generic;
using DomainModels;

namespace DomainServices.Services;

public interface ICustomerService
{
    (bool isValid, string message) CreateCustomer(Customer customer);

    IList<Customer> GetCustomers();

    Customer GetById(Guid CustomerId);

    Customer GetByFullName(string FullName);

    (bool isValid, string message) Update(Customer customer);

    bool Delete(Guid id);
}
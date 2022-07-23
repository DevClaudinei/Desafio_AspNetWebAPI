using System;
using System.Collections.Generic;
using DomainModels;

namespace DomainServices.Services;

public interface ICustomerService
{
    (bool isValid, string message) CreateCustomer(Customer customer);

    IEnumerable<Customer> GetAll();

    Customer GetById(Guid id);

    Customer GetByFullName(string fullName);

    (bool isValid, string message) Update(Customer customer);

    bool Delete(Guid id);
}
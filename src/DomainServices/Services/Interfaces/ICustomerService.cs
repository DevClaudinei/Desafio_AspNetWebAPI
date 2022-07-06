using System;
using System.Collections.Generic;
using DomainModels;

namespace DomainServices.Services;

public interface ICustomerService<T> where T : Customer 
{
    (bool isValid, string message) CreateCustomer(Customer customer);

    IEnumerable<T> GetCustomers();

    T GetById(Guid CustomerId);

    T GetByFullName(string FullName);

    (bool isValid, string message) Update(Customer customer);

    bool Delete(Guid id);
}
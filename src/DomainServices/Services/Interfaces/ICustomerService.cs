using System;
using System.Collections.Generic;
using DomainModels.Entities;

namespace DomainServices.Services;

public interface ICustomerService
{
    (bool isValid, string message) CreateCustomer(Customer customer);
    (bool isValid, string message) CreatePortfolio(Portfolio portfolio);
    IEnumerable<Customer> GetAll();
    Customer GetById(Guid id);
    IEnumerable<Customer> GetAllByFullName(string fullName);
    (bool isValid, string message) Update(Customer customer);
    bool Delete(Guid id);
}
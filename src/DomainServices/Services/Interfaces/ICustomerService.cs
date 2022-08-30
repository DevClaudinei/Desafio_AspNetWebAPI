using System.Collections.Generic;
using DomainModels.Entities;

namespace DomainServices.Services;

public interface ICustomerService
{
    (bool isValid, string message) CreateCustomer(Customer customer);
    IEnumerable<Customer> GetAll();
    Customer GetById(long id);
    IEnumerable<Customer> GetAllByFullName(string fullName);
    (bool isValid, string message) Update(Customer customer);
    (bool isValid, string message) Delete(long id);
}
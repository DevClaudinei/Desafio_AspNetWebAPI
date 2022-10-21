using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services;

public interface ICustomerService
{
    long CreateCustomer(Customer customer);
    IEnumerable<Customer> GetAll();
    Customer GetById(long id);
    IEnumerable<Customer> GetAllByFullName(string fullName);
    void Update(long id, Customer customer);
    void Delete(long id);
}
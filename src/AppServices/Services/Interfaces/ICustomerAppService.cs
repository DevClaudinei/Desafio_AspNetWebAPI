using System;
using System.Collections.Generic;
using DomainModels;

namespace AppServices;
public interface ICustomerAppService
{
    bool CreateCustomer(Customer customer);

    IList<Customer> GetCustomers();

    Customer GetCustomerByCpf(string Cpf);

    Customer GetCustomerByEmail(string Email);

    Customer GetCustomerById(Guid Id);

    Customer GetCustomerByName(string FullName);

    Customer UpdateCustomer(Customer customer);

    bool ExcludeCustomer(Guid id);
}
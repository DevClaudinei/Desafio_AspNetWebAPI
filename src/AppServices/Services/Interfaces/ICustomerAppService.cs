using System;
using System.Collections.Generic;
using DomainModels;

namespace AppServices;

public interface ICustomerAppService
{
    (bool isValid, string message) CreateCustomer(Customer customer);

    IEnumerable<CustomerViewModel> GetCustomers();

    bool GetCustomerByCpf(string cpf);

    bool GetCustomerByEmail(string email);

    bool GetCustomerById(Guid id);

    bool GetCustomerByName(string fullName);

    Customer UpdateCustomer(Customer customer);

    bool ExcludeCustomer(Guid id);
}
using System;
using System.Collections.Generic;
using DomainModels;
using DomainModels.Models;

namespace AppServices;

public interface ICustomerAppService
{
    (bool isValid, string message) CreateCustomer(CustomerToCreate customerToCreate);

    IEnumerable<CustomerViewModel> GetCustomers();

    bool GetCustomerByCpf(string cpf);

    bool GetCustomerByEmail(string email);

    CustomerViewModel GetCustomerById(Guid id);

    CustomerViewModel GetCustomerByName(string fullName);

    Customer UpdateCustomer(CustomerToUpdate customerToUpdate);

    bool ExcludeCustomer(Guid id);
}
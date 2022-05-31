using System;
using System.Collections.Generic;
using Application.Models;
using DomainModels;

namespace AppServices;

public interface ICustomerAppService
{
    (bool isValid, string message) CreateCustomer(CustomerToCreate customerToCreate);

    IEnumerable<CustomerViewModel> GetCustomers();

    bool CheckForAClientWithCpf(string cpf);

    bool CheckForAClientWithEmail(string email);

    CustomerViewModel GetCustomerById(Guid id);

    CustomerViewModel GetCustomerByName(string fullName);

    Customer UpdateCustomer(CustomerToUpdate customerToUpdate);

    bool DeleteCustomer(Guid id);
}
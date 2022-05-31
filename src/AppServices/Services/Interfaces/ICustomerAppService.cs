using System;
using System.Collections.Generic;
using Application.Models;
using DomainModels;

namespace AppServices.Services;

public interface ICustomerAppService
{
    (bool isValid, string message) Create(CreateCustomerRequest createCustomerRequest);

    IEnumerable<CustomerResult> Get();

    bool AnyCustomerForCpf(string cpf);

    bool AnyCustomerForEmail(string email);

    CustomerResult GetCustomerById(Guid id);

    CustomerResult GetCustomerByName(string fullName);

    Customer Update(UpdateCustomerRequest updateCustomerRequest);

    bool Delete(Guid id);
}
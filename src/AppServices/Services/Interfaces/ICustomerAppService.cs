using System;
using System.Collections.Generic;
using Application.Models;
using DomainModels;

namespace AppServices.Services;

public interface ICustomerAppService
{
    (bool isValid, string message) Create(CreateCustomerRequest createCustomerRequest);

    IEnumerable<CustomerResult> Get();

    CustomerResult GetCustomerById(Guid id);

    CustomerResult GetCustomerByName(string fullName);

    (bool isValid, string message) Update(UpdateCustomerRequest updateCustomerRequest);

    bool Delete(Guid id);
}
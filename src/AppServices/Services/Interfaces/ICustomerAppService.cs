using System;
using System.Collections.Generic;
using Application.Models;
using DomainModels.Entities;

namespace AppServices.Services;

public interface ICustomerAppService
{
    (bool isValid, string message) Create(CreateCustomerRequest createCustomerRequest);
    (bool isValid, string message) CreatePortfolio(Portfolio portfolio);
    IEnumerable<CustomerResult> Get();
    CustomerResult GetCustomerById(Guid id);
    IEnumerable<CustomerResult> GetAllCustomerByName(string fullName);
    (bool isValid, string message) Update(UpdateCustomerRequest updateCustomerRequest);
    bool Delete(Guid id);
}
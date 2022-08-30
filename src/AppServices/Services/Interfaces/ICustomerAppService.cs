using System.Collections.Generic;
using Application.Models;

namespace AppServices.Services;

public interface ICustomerAppService
{
    (bool isValid, string message) Create(CreateCustomerRequest createCustomerRequest);
    IEnumerable<CustomerResult> Get();
    CustomerResult GetCustomerById(long id);
    IEnumerable<CustomerResult> GetAllCustomerByName(string fullName);
    (bool isValid, string message) Update(UpdateCustomerRequest updateCustomerRequest);
    (bool isValid, string message) Delete(long id);
}
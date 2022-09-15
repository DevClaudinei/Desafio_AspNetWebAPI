using System.Collections.Generic;
using Application.Models;

namespace AppServices.Services;

public interface ICustomerAppService
{
    long Create(CreateCustomerRequest createCustomerRequest);
    IEnumerable<CustomerResult> Get();
    CustomerResult GetCustomerById(long id);
    IEnumerable<CustomerResult> GetAllCustomerByName(string fullName);
    void Update(long id, UpdateCustomerRequest updateCustomerRequest);
    void Delete(long id);
}
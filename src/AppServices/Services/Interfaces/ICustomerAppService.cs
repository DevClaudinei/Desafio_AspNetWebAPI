using System.Collections.Generic;
using Application.Models;

namespace AppServices.Services;

public interface ICustomerAppService
{
    long Create(CreateCustomerRequest createCustomerRequest);
    IEnumerable<CustomerResult> Get();
    CustomerResult GetById(long id);
    IEnumerable<CustomerResult> GetByName(string fullName);
    void Update(long id, UpdateCustomerRequest updateCustomerRequest);
    void Delete(long id);
}
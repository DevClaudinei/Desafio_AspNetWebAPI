using Application.Models.Customer.Requests;
using Application.Models.Customer.Response;
using System.Collections.Generic;

namespace AppServices.Services;

public interface ICustomerAppService
{
    long Create(CreateCustomerRequest createCustomerRequest);
    IEnumerable<CustomerResult> GetAll();
    CustomerResult GetById(long id);
    IEnumerable<CustomerResult> GetByName(string fullName);
    void Update(long id, UpdateCustomerRequest updateCustomerRequest);
    void Delete(long id);
}
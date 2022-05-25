using System;
using System.Collections.Generic;
using AutoMapper;
using DomainModels;
using DomainServices.Services;

namespace AppServices;

public class CustomerAppService : ICustomerAppService
{
    private readonly ICustomerService _customerService;
    private readonly IMapper _mapper;

    public CustomerAppService(ICustomerService customerService, IMapper mapper)
    {
        _customerService = customerService;
        _mapper = mapper;
    }

    public bool CreateCustomer(Customer customer)
    {
        var CreatedCustomer = _customerService.CreateCustomer(customer);
        if (CreatedCustomer is false) return false;
        return CreatedCustomer;
        
    }

    public IList<Customer> GetCustomers()
    {
        var CustomersFound = _customerService.GetCustomers();
        var CustomerMapper = _mapper.Map<CustomerViewModel>(CustomersFound);
        return (IList<Customer>)CustomerMapper;
    }

    public Customer GetCustomerByCpf(string Cpf)
    {
        var FoundCustomer = _customerService.GetCustomerByCpf(Cpf);
        return FoundCustomer;
    }

    public Customer GetCustomerByEmail(string Email)
    {
        var FoundCustomer = _customerService.GetCustomerByEmail(Email);
        return FoundCustomer;
    }

    public Customer GetCustomerById(Guid Id)
    {
        var FoundCustomer = _customerService.GetCustomerById(Id);
        return FoundCustomer;
    }

    public Customer GetCustomerByName(string FullName)
    {
        var FoundCustomer = _customerService.GetCustomerByName(FullName);
        return FoundCustomer;
    }

    public Customer UpdateCustomer(Customer customer)
    {
        var UpdatedCustomer = _customerService.UpdateCustomer(customer);
        return UpdatedCustomer;
    }

    public bool ExcludeCustomer(Guid id)
    {
        var DeletedCustomer = _customerService.ExcludeCustomer(id);
        return DeletedCustomer;
    }
}
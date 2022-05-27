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

    public (bool isValid, string message) CreateCustomer(Customer customer)
    {
        customer.Id = Guid.NewGuid();
        var CreatedCustomer = _customerService.CreateCustomer(customer);
        if (CreatedCustomer.isValid) return (true, customer.Id.ToString());

        return (false, CreatedCustomer.message); 
    }

    public IEnumerable<CustomerViewModel> GetCustomers()
    {
        var CustomersFound = _customerService.GetCustomers();
        return _mapper.Map<IEnumerable<CustomerViewModel>>(CustomersFound);
    }

    public bool GetCustomerByCpf(string cpf)
    {
        return _customerService.CustomerForCpfAlreadyExists(cpf);
    }

    public bool GetCustomerByEmail(string email)
    {
        return _customerService.CustomerForEmailAlreadyExists(email);
    }

    CustomerViewModel ICustomerAppService.GetCustomerById(Guid id)
    {
        var FoundCustomer = _customerService.GetCustomerById(id);
        if (FoundCustomer is null) return null;

        return _mapper.Map<CustomerViewModel>(FoundCustomer);
    }

    CustomerViewModel ICustomerAppService.GetCustomerByName(string fullName)
    {
        var FoundCustomer = _customerService.GetCustomerByName(fullName);
        if (FoundCustomer is null) return null;

        return _mapper.Map<CustomerViewModel>(FoundCustomer);
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
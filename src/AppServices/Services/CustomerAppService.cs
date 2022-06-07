using System;
using System.Collections.Generic;
using Application.Models;
using AutoMapper;
using DomainModels;
using DomainServices.Services;

namespace AppServices.Services;

public class CustomerAppService : ICustomerAppService
{
    private readonly ICustomerService _customerService;
    private readonly IMapper _mapper;

    public CustomerAppService(ICustomerService customerService, IMapper mapper)
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public (bool isValid, string message) Create(CreateCustomerRequest createCustomerRequest)
    {
        var customer = _mapper.Map<Customer>(createCustomerRequest);
        var createdCustomer = _customerService.CreateCustomer(customer);
        if (createdCustomer.isValid) return (true, createdCustomer.message);

        return (false, createdCustomer.message);
    }

    public IEnumerable<CustomerResult> Get()
    {
        var customersFound = _customerService.GetCustomers();
        return _mapper.Map<IEnumerable<CustomerResult>>(customersFound);
    }

    public CustomerResult GetCustomerById(Guid id)
    {
        var customer = _customerService.GetById(id);
        if (customer is null) return null;

        return _mapper.Map<CustomerResult>(customer);
    }

    public CustomerResult GetCustomerByName(string fullName)
    {
        var customer = _customerService.GetByFullName(fullName);
        if (customer is null) return null;

        return _mapper.Map<CustomerResult>(customer);
    }

    public (bool isValid, string message) Update(UpdateCustomerRequest updateCustomerRequest)
    {
        var customerToUpdate = _mapper.Map<Customer>(updateCustomerRequest);
        return _customerService.Update(customerToUpdate);
    }

    public bool Delete(Guid id)
    {
        var deletedCustomer = _customerService.Delete(id);
        return deletedCustomer;
    }
}
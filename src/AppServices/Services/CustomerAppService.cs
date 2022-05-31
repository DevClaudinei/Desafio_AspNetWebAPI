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

    public bool AnyCustomerForCpf(string cpf)
    {
        return _customerService.CustomerForCpfAlreadyExists(cpf);
    }

    public bool AnyCustomerForEmail(string email)
    {
        return _customerService.CustomerForEmailAlreadyExists(email);
    }

    public CustomerResult GetCustomerById(Guid id)
    {
        var foundCustomer = _customerService.GetCustomerById(id);
        if (foundCustomer is null) return null;

        return _mapper.Map<CustomerResult>(foundCustomer);
    }

    public CustomerResult GetCustomerByName(string fullName)
    {
        var foundCustomer = _customerService.GetCustomerByName(fullName);
        if (foundCustomer is null) return null;

        return _mapper.Map<CustomerResult>(foundCustomer);
    }

    public Customer Update(UpdateCustomerRequest updateCustomerRequest)
    {
        var conversationBetweenEntities = _mapper.Map<Customer>(updateCustomerRequest);
        var updatedCustomer = _customerService.UpdateCustomer(conversationBetweenEntities);
        return _mapper.Map<Customer>(updatedCustomer);
    }

    public bool Delete(Guid id)
    {
        var deletedCustomer = _customerService.DeleteCustomer(id);
        return deletedCustomer;
    }
}
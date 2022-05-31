using System;
using System.Collections.Generic;
using Application.Models;
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
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _mapper = mapper;
    }

    public (bool isValid, string message) CreateCustomer(CustomerToCreate customerToCreate)
    {
        customerToCreate.Id = Guid.NewGuid();
        var CreatedCustomer = _customerService.CreateCustomer(_mapper.Map<Customer>(customerToCreate));
        if (CreatedCustomer.isValid) return (true, customerToCreate.Id.ToString());
        
        return (false, CreatedCustomer.message); 
    }

    public IEnumerable<CustomerViewModel> GetCustomers()
    {
        var CustomersFound = _customerService.GetCustomers();
        return _mapper.Map<IEnumerable<CustomerViewModel>>(CustomersFound);
    }

    public bool CheckForAClientWithCpf(string cpf)
    {
        return _customerService.CustomerForCpfAlreadyExists(cpf);
    }

    public bool CheckForAClientWithEmail(string email)
    {
        return _customerService.CustomerForEmailAlreadyExists(email);
    }

    public CustomerViewModel GetCustomerById(Guid id)
    {
        var FoundCustomer = _customerService.GetCustomerById(id);
        if (FoundCustomer is null) return null;

        return _mapper.Map<CustomerViewModel>(FoundCustomer);
    }

    public CustomerViewModel GetCustomerByName(string fullName)
    {
        var FoundCustomer = _customerService.GetCustomerByName(fullName);
        if (FoundCustomer is null) return null;

        return _mapper.Map<CustomerViewModel>(FoundCustomer);
    }

    public Customer UpdateCustomer(CustomerToUpdate customerToUpdate)
    {
        var conversationBetweenEntities = _mapper.Map<Customer>(customerToUpdate);
        var updatedCustomer = _customerService.UpdateCustomer(conversationBetweenEntities);
        return _mapper.Map<Customer>(updatedCustomer); 
    }

    public bool DeleteCustomer(Guid id)
    {
        var DeletedCustomer = _customerService.DeleteCustomer(id);
        return DeletedCustomer;
    }
}
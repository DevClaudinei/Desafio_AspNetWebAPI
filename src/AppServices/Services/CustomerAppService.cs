using System;
using System.Collections.Generic;
using Application.Models;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Services;

namespace AppServices.Services;

public class CustomerAppService : ICustomerAppService
{
    private readonly IMapper _mapper;
    private readonly ICustomerService _customerService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;
    private readonly IPortfolioAppService _portfolioAppService;
    
    public CustomerAppService(
        ICustomerService customerService,
        IMapper mapper,
        ICustomerBankInfoAppService customerBankInfoService,
        IPortfolioAppService portfolioService
    )
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _customerBankInfoAppService = customerBankInfoService ?? throw new ArgumentNullException(nameof(customerBankInfoService));
        _portfolioAppService = portfolioService ?? throw new ArgumentNullException(nameof(portfolioService));
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
        var customersFound = _customerService.GetAll();
        return _mapper.Map<IEnumerable<CustomerResult>>(customersFound);
    }

    public CustomerResult GetCustomerById(Guid id)
    {
        var customer = _customerService.GetById(id);
        if (customer is null) return null;

        return _mapper.Map<CustomerResult>(customer);
    }

    public IEnumerable<CustomerResult> GetAllCustomerByName(string fullName)
    {
        var customer = _customerService.GetAllByFullName(fullName);
        if (customer is null) return null;

        return _mapper.Map<IEnumerable<CustomerResult>>(customer);
    }

    public (bool isValid, string message) Update(UpdateCustomerRequest updateCustomerRequest)
    {
        var customerToUpdate = _mapper.Map<Customer>(updateCustomerRequest);
        return _customerService.Update(customerToUpdate);
    }

    public (bool isValid, string message) Delete(Guid id)
    {
        var customerBankInfoBalance = _customerBankInfoAppService.GetAllCustomerBankInfo();
        foreach (var item in customerBankInfoBalance)
        {
            if (item.AccountBalance > 0)
            {
                return (false, $"Customer precisa resgatar seu saldo antes de ser excluido.");
            }
        }

        var customerTotaltBalance = _portfolioAppService.GetAllPortfolios();

        foreach (var item in customerTotaltBalance)
        {
            if (item.TotalBalance > 0)
            {
                return (false, $"Customer precisa resgatar seu saldo antes de ser excluido.");
            }
        }

        var deletedCustomer = _customerService.Delete(id);
        return deletedCustomer;
    }

}
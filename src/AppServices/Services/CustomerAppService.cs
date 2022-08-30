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
        ICustomerBankInfoAppService customerBankInfoAppService,
        IPortfolioAppService portfolioAppService
    )
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
        _portfolioAppService = portfolioAppService ?? throw new ArgumentNullException(nameof(portfolioAppService));
    }

    public (bool isValid, string message) Create(CreateCustomerRequest createCustomerRequest)
    {
        var customer = _mapper.Map<Customer>(createCustomerRequest);
        var createdCustomer = _customerService.CreateCustomer(customer);

        if (createdCustomer.isValid)
        {
            var customerBankInfo = _customerBankInfoAppService.Create(createCustomerRequest.CustomerBaninfo, long.Parse(createdCustomer.message));
            if (customerBankInfo.isValid) return (true, createdCustomer.message);
        }

        return (false, createdCustomer.message);
    }

    public IEnumerable<CustomerResult> Get()
    {
        var customersFound = _customerService.GetAll();
        return _mapper.Map<IEnumerable<CustomerResult>>(customersFound);
    }

    public CustomerResult GetCustomerById(long id)
    {
        var customerFound = _customerService.GetById(id);
        if (customerFound is null) return null;

        return _mapper.Map<CustomerResult>(customerFound);
    }

    public IEnumerable<CustomerResult> GetAllCustomerByName(string fullName)
    {
        var customersFound = _customerService.GetAllByFullName(fullName);
        if (customersFound is null) return null;

        return _mapper.Map<IEnumerable<CustomerResult>>(customersFound);
    }

    public (bool isValid, string message) Update(UpdateCustomerRequest updateCustomerRequest)
    {
        var customerToUpdate = _mapper.Map<Customer>(updateCustomerRequest);
        return _customerService.Update(customerToUpdate);
    }

    public (bool isValid, string message) Delete(long id)
    {
        var customerAccountBalance = _customerBankInfoAppService.GetAllCustomerBankInfo();
       
        foreach (var item in customerAccountBalance)
        {
            if (item.CustomerId != id) return (false, $"Customer para o Id: {id} não localizado.");
            if (item.AccountBalance > 0) return (false, $"Customer precisa resgatar seu saldo antes de ser excluido.");
        }

        var customerTotaltBalance = _portfolioAppService.GetAllPortfolios();

        foreach (var item in customerTotaltBalance)
        {
            if (item.TotalBalance > 0) return (false, $"Customer precisa resgatar seu saldo antes de ser excluido.");
        }

        var deletedCustomer = _customerService.Delete(id);

        return deletedCustomer;
    }

}
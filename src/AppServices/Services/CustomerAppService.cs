using Application.Models;
using Application.Models.Portfolio.Response;
using Application.Models.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppServices.Services;

public class CustomerAppService : ICustomerAppService
{
    private readonly IMapper _mapper;
    private readonly ICustomerService _customerService;
    private readonly IPortfolioAppService _portfolioAppService;
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;
    
    public CustomerAppService(
        IMapper mapper,
        ICustomerService customerService,
        IPortfolioAppService portfolioAppService,
        ICustomerBankInfoAppService customerBankInfoAppService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _portfolioAppService = portfolioAppService ?? throw new ArgumentNullException(nameof(portfolioAppService));
        _customerBankInfoAppService = customerBankInfoAppService ?? throw new ArgumentNullException(nameof(customerBankInfoAppService));
    }

    public long Create(CreateCustomerRequest createCustomerRequest)
    {
        var customer = _mapper.Map<Customer>(createCustomerRequest);
        var createdCustomer = _customerService.CreateCustomer(customer);

        _customerBankInfoAppService.Create(customer.Id);
        return createdCustomer;
    }

    public IEnumerable<CustomerResult> Get()
    {
        var customersFound = _customerService.GetAll();
        return _mapper.Map<IEnumerable<CustomerResult>>(customersFound);
    }

    public CustomerResult GetCustomerById(long id)
    {
        var customerFound = _customerService.GetById(id);
        if (customerFound is null) throw new NotFoundException($"Customer for Id: {id} was not found.");

        return _mapper.Map<CustomerResult>(customerFound);
    }

    public IEnumerable<CustomerResult> GetAllCustomerByName(string fullName)
    {
        var customersFound = _customerService.GetAllByFullName(fullName);
        if (customersFound.Count() == 0) throw new NotFoundException($"Client for name: {fullName} could not be found.");

        return _mapper.Map<IEnumerable<CustomerResult>>(customersFound);
    }

    public void Update(long id, UpdateCustomerRequest updateCustomerRequest)
    {
        var customerToUpdate = _mapper.Map<Customer>(updateCustomerRequest);
        _customerService.Update(id, customerToUpdate);
    }

    public void Delete(long id)
    {
        var customerAccountBalance = _customerBankInfoAppService.GetAllCustomerBankInfo();
        var customerTotaltBalance = _portfolioAppService.GetAllPortfolios();
        
        CheckCustomerBankInfoAccountBalance(customerAccountBalance, id);
        CheckPortfolioTotalBalance(customerTotaltBalance, id);
       
        _customerService.Delete(id);
    }

    private bool CheckPortfolioTotalBalance(IEnumerable<PortfolioResult> portfoliosInfo, long id)
    {
        var exclusionOfValidCustomer = false;

        foreach (var item in portfoliosInfo)
        {
            if (item.CustomerId != id) continue;

            if (item.CustomerId == id && item.TotalBalance > 0) 
                throw new BadRequestException($"Customer needs to redeem their balance before being deleted.");

            if (item.CustomerId == id && item.TotalBalance == 0) exclusionOfValidCustomer = true;
        }

        return exclusionOfValidCustomer;
    }

    private bool CheckCustomerBankInfoAccountBalance(IEnumerable<CustomerBankInfoResult> customerBankInfos, long id)
    {
        var exclusionOfValidCustomer = false;

        foreach (var item in customerBankInfos)
        {
            if (item.CustomerId != id) continue;

            if (item.CustomerId == id && item.AccountBalance > 0)
                throw new BadRequestException($"Customer needs to redeem their balance before being deleted.");

            if (item.CustomerId == id && item.AccountBalance == 0) exclusionOfValidCustomer = true;
        }

        return exclusionOfValidCustomer;
    }
}
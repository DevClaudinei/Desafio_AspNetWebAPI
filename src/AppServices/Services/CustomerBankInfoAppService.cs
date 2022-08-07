using Application.Models;
using Application.Models.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class CustomerBankInfoAppService : ICustomerBankInfoAppService
{
    private readonly ICustomerBankInfoService _customerService;
    private readonly IMapper _mapper;

    public CustomerBankInfoAppService(ICustomerBankInfoService customerService, IMapper mapper)
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public (bool isValid, string message) Create(CreateCustomerBankInfoRequest createCustomerBankInfoRequest)
    {
        var customerBankInfo = _mapper.Map<CustomerBankInfo>(createCustomerBankInfoRequest);
        var createdCustomerBankInfo = _customerService.CreateCustomerBankInfo(customerBankInfo);

        if (createdCustomerBankInfo.isValid) return (true, createdCustomerBankInfo.message);

        return (false, createdCustomerBankInfo.message);
    }

    public IEnumerable<CustomerBankInfoResult> Get()
    {
        var customersBankInfo = _customerService.GetAllCustomerBankInfo();
        return _mapper.Map<IEnumerable<CustomerBankInfoResult>>(customersBankInfo);
    }

    public CustomerBankInfoResult GetAllCustomerBackInfoByAccount(string account)
    {
        var customerBankInfo = _customerService.GetCustomerBankInfoByAccount(account);
        if (customerBankInfo is null) return null;

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }

    public CustomerBankInfoResult GetCustomerBankInfoById(Guid id)
    {
        var customerBankInfo = _customerService.GetCustomerBankInfoById(id);
        if (customerBankInfo is null) return null;

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }

    public (bool isValid, string message) Update(UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        
        var customerBankInfoToUpdate = _mapper.Map<CustomerBankInfo>(updateCustomerBankInfoRequest);
        return _customerService.UpdateCustomerBankInfo(customerBankInfoToUpdate);
    }

    public bool Delete(Guid id)
    {
        var deletedCustomer = _customerService.Delete(id);
        return deletedCustomer;
    }
}

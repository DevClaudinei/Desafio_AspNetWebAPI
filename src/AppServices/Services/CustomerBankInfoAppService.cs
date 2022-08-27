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
    private readonly ICustomerBankInfoService _customerBankInfoService;
    private readonly IMapper _mapper;

    public CustomerBankInfoAppService(ICustomerBankInfoService customerBankInfoService, IMapper mapper)
    {
        _customerBankInfoService = customerBankInfoService ?? throw new ArgumentNullException(nameof(customerBankInfoService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public (bool isValid, string message) Create(CreateCustomerBankInfoRequest createCustomerBankInfoRequest, Guid customerId)
    {
        var customerBankInfo = _mapper.Map<CustomerBankInfo>(createCustomerBankInfoRequest);
        var createdCustomerBankInfo = _customerBankInfoService.CreateCustomerBankInfo(customerBankInfo, customerId);

        if (createdCustomerBankInfo.isValid) return (true, createdCustomerBankInfo.message);

        return (false, createdCustomerBankInfo.message);
    }

    public IEnumerable<CustomerBankInfoResult> GetAllCustomerBankInfo()
    {
        var customersBankInfo = _customerBankInfoService.GetAllCustomerBankInfo();
        return _mapper.Map<IEnumerable<CustomerBankInfoResult>>(customersBankInfo);
    }

    public CustomerBankInfoResult GetAllCustomerBackInfoByAccount(string account)
    {
        var customerBankInfo = _customerBankInfoService.GetCustomerBankInfoByAccount(account);
        if (customerBankInfo is null) return null;

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }

    public CustomerBankInfoResult GetCustomerBankInfoById(Guid id)
    {
        var customerBankInfo = _customerBankInfoService.GetCustomerBankInfoById(id);
        if (customerBankInfo is null) return null;

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }

    public (bool isValid, string message) DepositMoney(UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        var customerBankInfoToUpdate = _mapper.Map<CustomerBankInfo>(updateCustomerBankInfoRequest);
        return _customerBankInfoService.DepositMoney(customerBankInfoToUpdate);
    }

    public (bool isValid, string message) WithdrawMoney(UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        var customerBankInfoToUpdate = _mapper.Map<CustomerBankInfo>(updateCustomerBankInfoRequest);
        return _customerBankInfoService.WithdrawMoney(customerBankInfoToUpdate);
    }

    public (bool isValid, string message) Delete(Guid id)
    {
        return _customerBankInfoService.Delete(id);
    }
}

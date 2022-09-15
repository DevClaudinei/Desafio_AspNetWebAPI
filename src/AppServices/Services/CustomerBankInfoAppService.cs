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

    public CustomerBankInfoResult GetCustomerBankInfoById(long id)
    {
        var customerBankInfo = _customerBankInfoService.GetCustomerBankInfoById(id);
        if (customerBankInfo is null) return null;

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }

    public void DepositMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        var customerBankInfoToUpdate = _mapper.Map<CustomerBankInfo>(updateCustomerBankInfoRequest);
        _customerBankInfoService.DepositMoney(id, customerBankInfoToUpdate);
    }

    public void WithdrawMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        var customerBankInfoToUpdate = _mapper.Map<CustomerBankInfo>(updateCustomerBankInfoRequest);
        _customerBankInfoService.WithdrawMoney(id, customerBankInfoToUpdate);
    }

    public bool UpdateBalanceAfterPurchase(CustomerBankInfoResult customerBankinfo, decimal purchaseValue)
    {
        customerBankinfo.AccountBalance -= purchaseValue;
        var customerBankInfoToUpdate = _mapper.Map<CustomerBankInfo>(customerBankinfo);
        return _customerBankInfoService.UpdateBalanceAfterPurchase(customerBankInfoToUpdate);
    }

    public void Create(long customerId)
    {
        _customerBankInfoService.Create(customerId);
    }
}

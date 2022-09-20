using Application.Models;
using Application.Models.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class CustomerBankInfoAppService : ICustomerBankInfoAppService
{
    private readonly IMapper _mapper;
    private readonly ICustomerBankInfoService _customerBankInfoService;

    public CustomerBankInfoAppService(ICustomerBankInfoService customerBankInfoService, IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _customerBankInfoService = customerBankInfoService ?? throw new ArgumentNullException(nameof(customerBankInfoService));
    }

    public IEnumerable<CustomerBankInfoResult> GetAllCustomerBankInfo()
    {
        var customersBankInfo = _customerBankInfoService.Get();
        return _mapper.Map<IEnumerable<CustomerBankInfoResult>>(customersBankInfo);
    }

    public CustomerBankInfoResult GetAllCustomerBackInfoByAccount(string account)
    {
        var customerBankInfo = _customerBankInfoService.GetCustomerBankInfoByAccount(account);
        if (customerBankInfo is null) throw new GenericNotFoundException($"CustomerBankInfo for account: {account} was not found.");

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }

    public CustomerBankInfoResult Get(long id)
    {
        var customerBankInfo = _customerBankInfoService.GetCustomerBankInfoById(id);
        if (customerBankInfo is null) throw new GenericNotFoundException($"CustomerBankInfo for id: {id} not found.");

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }

    private CustomerBankInfo GetById(long id)
    {
        var customerBankInfo = _customerBankInfoService.GetCustomerBankInfoById(id);
        if (customerBankInfo is null) throw new GenericNotFoundException($"CustomerBankInfo for id: {id} not found.");

        return customerBankInfo;
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

    public bool UpdateBalanceAfterPurchase(long customerBankInfoId, decimal purchaseValue)
    {
        var customerBankinfo = GetById(customerBankInfoId);
        customerBankinfo.AccountBalance -= purchaseValue;
        var customerBankInfoToUpdate = _mapper.Map<CustomerBankInfo>(customerBankinfo);
        return _customerBankInfoService.UpdateBalanceAfterPurchase(customerBankInfoToUpdate);
    }

    public bool UpdateBalanceAfterRescue(CustomerBankInfo customerBankinfo, decimal purchaseValue)
    {
        customerBankinfo.AccountBalance += purchaseValue;
        return _customerBankInfoService.UpdateBalanceAfterPurchase(customerBankinfo);
    }

    public void Create(long customerId)
    {
        _customerBankInfoService.Create(customerId);
    }

    public bool RedeemInvestedAmount(long customerId, decimal purchaseValue) 
    {
        var customerBankInfos = _customerBankInfoService.Get();

        foreach (var customerBankInfo in customerBankInfos)
        {
            if (customerBankInfo.CustomerId == customerId) 
                UpdateBalanceAfterRescue(customerBankInfo, purchaseValue);
        }

        return true;
    }

    public bool CheckCustomerAccountBalance(decimal netValue, long customerBankInfoId)
    {
        var customerBankInfo = Get(customerBankInfoId);
        if (customerBankInfo.AccountBalance < netValue) return false;
            
        return true;
    }
}
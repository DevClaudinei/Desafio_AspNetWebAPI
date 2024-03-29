﻿using Application.Models.CustomerBackInfo.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
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

    public IEnumerable<CustomerBankInfoResult> GetAll()
    {
        var customersBankInfo = _customerBankInfoService.GetAll();
        return _mapper.Map<IEnumerable<CustomerBankInfoResult>>(customersBankInfo);
    }

    public CustomerBankInfoResult GetByAccount(string account)
    {
        var customerBankInfo = _customerBankInfoService.GetByAccount(account)
            ?? throw new NotFoundException($"CustomerBankInfo for account: {account} could not be found.");

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }

    public CustomerBankInfoResult GetById(long id)
    {
        var customerBankInfo = _customerBankInfoService.GetById(id)
            ?? throw new NotFoundException($"CustomerBankInfo for id: {id} not found.");

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }

    public long GetCustomerBankInfoId(long customerId)
    {
        return _customerBankInfoService.GetIdByCustomerId(customerId);
    }

    public void Deposit(long id, decimal amount)
    {
        _customerBankInfoService.Deposit(id, amount);
    }

    public void Withdraw(long id, decimal amount)
    {
        _customerBankInfoService.Withdraw(id, amount);
    }

    public void Create(long customerId)
    {
        _customerBankInfoService.Create(customerId);
    }

    public void CanWithdrawAmountFromAccountBalance(decimal amount, long customerBankInfoId)
    {
        var accountBalance = _customerBankInfoService.GetAccountBalanceById(customerBankInfoId);

        if (accountBalance < amount) throw new BadRequestException($"Insufficient balance to invest.");
    }

    public CustomerBankInfoResult GetByCustomerId(long id)
    {
        var customerBankInfo = _customerBankInfoService.GetByCustomerId(id)
            ?? throw new NotFoundException($"CustomerBankInfo for CustomerId: {id}  could not be found.");

        return _mapper.Map<CustomerBankInfoResult>(customerBankInfo);
    }
}
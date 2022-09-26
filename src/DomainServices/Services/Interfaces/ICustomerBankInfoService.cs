﻿using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface ICustomerBankInfoService
{
    void Create(long customerId);
    IEnumerable<CustomerBankInfo> GetAll();
    CustomerBankInfo GetById(long id);
    CustomerBankInfo GetByAccount(string account);
    decimal GetAccountBalanceById(long id);
    void DepositMoney(long id, CustomerBankInfo customerBankInfo);
    void WithdrawMoney(long id, CustomerBankInfo customerBankInfo);
    bool UpdateBalanceAfterPurchase(CustomerBankInfo customerBankInfo);
    long RetornaIdDeContaAtravesDeCustomerID(long id);
}
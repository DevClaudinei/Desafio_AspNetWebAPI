using DomainModels.Entities;
using System;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface ICustomerBankInfoService
{
    (bool isValid, string message) CreateCustomerBankInfo(CustomerBankInfo customerBankInfo, long customerId);
    IEnumerable<CustomerBankInfo> GetAllCustomerBankInfo();
    CustomerBankInfo GetCustomerBankInfoById(long id);
    CustomerBankInfo GetCustomerBankInfoByAccount(string account);
    (bool isValid, string message) DepositMoney(CustomerBankInfo customerBankInfo);
    (bool isValid, string message) WithdrawMoney(CustomerBankInfo customerBankInfo);
    public bool UpdateBalanceAfterPurchase(CustomerBankInfo customerBankInfo);
    (bool isValid, string message) Delete(long id);
}

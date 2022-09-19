using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface ICustomerBankInfoService
{
    void Create(long customerId);
    IEnumerable<CustomerBankInfo> GetAllCustomerBankInfo();
    CustomerBankInfo GetCustomerBankInfoById(long id);
    CustomerBankInfo GetCustomerBankInfoByAccount(string account);
    void DepositMoney(long id, CustomerBankInfo customerBankInfo);
    void WithdrawMoney(long id, CustomerBankInfo customerBankInfo);
    public bool UpdateBalanceAfterPurchase(CustomerBankInfo customerBankInfo);
}
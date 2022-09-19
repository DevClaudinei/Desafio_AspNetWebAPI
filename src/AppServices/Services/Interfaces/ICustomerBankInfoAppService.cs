using Application.Models;
using Application.Models.Response;
using DomainModels.Entities;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface ICustomerBankInfoAppService
{
    void Create(long customerId);
    IEnumerable<CustomerBankInfoResult> GetAllCustomerBankInfo();
    CustomerBankInfoResult GetCustomerBankInfoById(long id);
    CustomerBankInfoResult GetAllCustomerBackInfoByAccount(string account);
    void DepositMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest);
    void WithdrawMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest);
    bool UpdateBalanceAfterPurchase(long customerBankId, decimal purchaseValue);
    bool UpdateBalanceAfterRescue(CustomerBankInfo customerBankinfo, decimal purchaseValue);
    bool RedeemInvestedAmount(long customerId, decimal purchaseValue);
    bool CheckCustomerAccountBalance(decimal netValue, long customerBankInfoId);
}
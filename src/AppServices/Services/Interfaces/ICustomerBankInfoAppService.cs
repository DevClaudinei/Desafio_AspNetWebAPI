using Application.Models.CustomerBackInfo.Requests;
using Application.Models.CustomerBackInfo.Response;
using DomainModels.Entities;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface ICustomerBankInfoAppService
{
    void Create(long customerId);
    IEnumerable<CustomerBankInfoResult> GetAll();
    CustomerBankInfoResult Get(long id);
    CustomerBankInfoResult GetByAccount(string account);
    void DepositMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest);
    void WithdrawMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest);
    bool UpdateBalanceAfterPurchase(long customerBankId, decimal purchaseValue);
    bool UpdateBalanceAfterRescue(CustomerBankInfo customerBankinfo, decimal purchaseValue);
    bool RedeemInvestedAmount(long customerId, decimal purchaseValue);
    bool CanWithdrawAmountFromAccountBalance(decimal netValue, long customerBankInfoId);
    long GetCustomerBankInfoId(long id);
}
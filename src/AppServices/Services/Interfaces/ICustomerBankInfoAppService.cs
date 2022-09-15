using Application.Models;
using Application.Models.Response;
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
    public bool UpdateBalanceAfterPurchase(CustomerBankInfoResult customerBankinfo, decimal purchaseValue);
    public bool UpdateBalanceAfterRescue(CustomerBankInfoResult customerBankinfo, decimal purchaseValue);
}

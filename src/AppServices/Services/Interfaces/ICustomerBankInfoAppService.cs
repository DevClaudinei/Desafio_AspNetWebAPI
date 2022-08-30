using Application.Models;
using Application.Models.Response;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface ICustomerBankInfoAppService
{
    (bool isValid, string message) Create(CreateCustomerBankInfoRequest createCustomerBankInfoRequest, long customerId);
    IEnumerable<CustomerBankInfoResult> GetAllCustomerBankInfo();
    CustomerBankInfoResult GetCustomerBankInfoById(long id);
    CustomerBankInfoResult GetAllCustomerBackInfoByAccount(string account);
    (bool isValid, string message) DepositMoney(UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest);
    (bool isValid, string message) WithdrawMoney(UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest);
    public bool UpdateBalanceAfterPurchase(CustomerBankInfoResult customerBankinfo, decimal purchaseValue);
    (bool isValid, string message) Delete(long id);
}

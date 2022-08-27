using Application.Models;
using Application.Models.Response;
using System;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface ICustomerBankInfoAppService
{
    (bool isValid, string message) Create(CreateCustomerBankInfoRequest createCustomerBankInfoRequest, Guid customerId);
    IEnumerable<CustomerBankInfoResult> GetAllCustomerBankInfo();
    CustomerBankInfoResult GetCustomerBankInfoById(Guid id);
    CustomerBankInfoResult GetAllCustomerBackInfoByAccount(string account);
    (bool isValid, string message) DepositMoney(UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest);
    (bool isValid, string message) WithdrawMoney(UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest);
    (bool isValid, string message) Delete(Guid id);
}

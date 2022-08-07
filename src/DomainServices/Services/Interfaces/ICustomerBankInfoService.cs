using DomainModels.Entities;
using System;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface ICustomerBankInfoService
{
    (bool isValid, string message) CreateCustomerBankInfo(CustomerBankInfo customerBankInfo);
    IEnumerable<CustomerBankInfo> GetAllCustomerBankInfo();
    CustomerBankInfo GetCustomerBankInfoById(Guid id);
    CustomerBankInfo GetCustomerBankInfoByAccount(string account);
    (bool isValid, string message) UpdateCustomerBankInfo(CustomerBankInfo customerBankInfo);
    bool Delete(Guid id);
}

using Application.Models.CustomerBackInfo.Response;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface ICustomerBankInfoAppService
{
    void Create(long customerId);
    IEnumerable<CustomerBankInfoResult> GetAll();
    CustomerBankInfoResult GetById(long id);
    CustomerBankInfoResult GetByAccount(string account);
    void Deposit(long id,decimal amount);
    void Withdraw(long id, decimal amount);
    void CanWithdrawAmountFromAccountBalance(decimal netValue, long customerBankInfoId);
    long GetCustomerBankInfoId(long id);
    CustomerBankInfoResult GetByCustomerId(long id);
}
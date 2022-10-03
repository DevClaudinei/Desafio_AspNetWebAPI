using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface ICustomerBankInfoService
{
    void Create(long customerId);
    IEnumerable<CustomerBankInfo> GetAll();
    CustomerBankInfo GetById(long id);
    CustomerBankInfo GetByAccount(string account);
    decimal GetAccountBalanceById(long id);
    void Deposit(long id, decimal amount);
    void Withdraw(long id, decimal amount);
    long RetornaIdDeContaAtravesDeCustomerID(long id);
}
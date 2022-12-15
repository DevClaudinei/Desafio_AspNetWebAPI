using Bogus;
using DomainModels.Entities;
using System;
using System.Collections.Generic;

namespace UnitTests.EntitiesFake.CustomerBankInfos;

public static class CustomerBankInfoFake
{
    public static CustomerBankInfo CustomerBankInfoFaker()
    {
        var customerBankInfoFake = new Faker<CustomerBankInfo>("pt_BR")
            .CustomInstantiator(f => new CustomerBankInfo(customerId: 1)).Generate();

        customerBankInfoFake.Account = Guid.NewGuid().ToString();
        customerBankInfoFake.AccountBalance = 0;
        customerBankInfoFake.Id = 1;
        customerBankInfoFake.CreatedAt = DateTime.UtcNow;

        return customerBankInfoFake;
    }

    public static IEnumerable<CustomerBankInfo> CustomerBankInfoFakers(int quantity)
    {
        var id = 1;
        var customerBankInfoFakes = new Faker<CustomerBankInfo>("pt_BR")
            .CustomInstantiator(f => new CustomerBankInfo(customerId: 1)).Generate(quantity);

        foreach (var customerBankInfo in customerBankInfoFakes)
        {
            customerBankInfo.Account = Guid.NewGuid().ToString();
            customerBankInfo.AccountBalance = 0;
            customerBankInfo.Id = id++;
            customerBankInfo.CreatedAt = DateTime.UtcNow;
        }

        return customerBankInfoFakes;
    }
}
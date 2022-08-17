﻿using Application.Models;
using Application.Models.Response;
using System;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface ICustomerBankInfoAppService
{
    (bool isValid, string message) Create(CreateCustomerBankInfoRequest createCustomerBankInfoRequest);
    IEnumerable<CustomerBankInfoResult> Get();
    CustomerBankInfoResult GetCustomerBankInfoById(Guid id);
    CustomerBankInfoResult GetAllCustomerBackInfoByAccount(string account);
    (bool isValid, string message) Update(UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest);
    bool Delete(Guid id);
}
using System;
using DesafioWarren.WebAPI.Models;

namespace DesafioWarren.WebAPI.Services;

public interface ICustomerService
{    
    bool CreateCustomer(Customer customer);

    Customer GetCustomerByEmail(string Email);

    Customer GetCustomerById(Guid CustomerId);

    Customer GetCustomerByName(string FullName);

    Customer UpdateCustomer(Customer customer);

    bool ExcludeCustomer(Guid id);
}
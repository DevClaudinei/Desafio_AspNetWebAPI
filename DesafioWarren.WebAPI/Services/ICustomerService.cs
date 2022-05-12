using System;
using System.Collections.Generic;
using DesafioWarren.WebAPI.Models;

namespace DesafioWarren.WebAPI.Services;

public interface ICustomerService
{
    public List<Customer> ListCustomer { get; set; }
    
    void CreateCustomer(Customer customer);

    Customer GetCustomerById(Guid CustomerId);

    Customer GetCustomerByName(string FullName);

    bool UpdateCustomer(Customer customer);

    bool ExcludeCustomer(Guid id);
}
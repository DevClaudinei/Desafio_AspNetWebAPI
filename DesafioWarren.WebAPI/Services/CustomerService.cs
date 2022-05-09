using System;
using System.Linq;
using DesafioWarren.WebAPI.Data;
using DesafioWarren.WebAPI.Models;

namespace DesafioWarren.WebAPI.Services
{
    public class CustomerService : ICustomerService
    {
        public void criaCustomer(Customer customer)
        { 
            BancoCustomer.ListCustomer.Add(customer);
        }

        public void atualizaCustomer(Customer customer)
        {
            var customerFound = BancoCustomer.ListCustomer.FirstOrDefault(a => a.Id == customer.Id);
            
            customerFound.Id = customer.Id;
            customerFound.FullName = customer.FullName;
            customerFound.Email = customer.Email;
            customerFound.EmailConfirmation = customerFound.EmailConfirmation;
            customerFound.Cpf = customer.Cpf;
            customerFound.Cellphone = customer.Cellphone;
            customerFound.Birthdate = customer.Birthdate;
            customerFound.EmailSms = customer.EmailSms;
            customerFound.Whatsapp = customer.Whatsapp;
            customerFound.Country = customer.Country;
            customerFound.City = customer.City;
            customerFound.PostalCode = customer.PostalCode;
            customerFound.Address = customer.Address;
            customerFound.Number = customer.Number;
        }

        public void excluiCustomer(Guid id)
        {
            var customerFound = BancoCustomer.ListCustomer.FirstOrDefault(a => a.Id.Equals(id));
            if (customerFound != null) BancoCustomer.ListCustomer.Remove(customerFound);
        }
    }
}
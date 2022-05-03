using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Services
{

    public class CustomerService : ICustomerService
    {

        public void criaCustomer(Customer customer)
        {
            SmartContext.ListCustomer.Add(customer);
        }

        public void atualizaCustomer(Customer customer)
        {
            var customerFound = SmartContext.ListCustomer.FirstOrDefault(a => a.Id == customer.Id);
            
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

        public void excluiCustomer(int id)
        {
            var customerFound = SmartContext.ListCustomer.FirstOrDefault(a => a.Id == id);
            if (customerFound != null) SmartContext.ListCustomer.Remove(customerFound);
        }

    }

}
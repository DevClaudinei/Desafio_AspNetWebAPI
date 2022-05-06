using System;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Services
{
    
    public interface ICustomerService
    {
        void criaCustomer(Customer customer);

        void atualizaCustomer(Customer customer);

        void excluiCustomer(Guid id);
    }

}
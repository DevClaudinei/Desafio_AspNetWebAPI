using System;
using DesafioWarren.WebAPI.Models;

namespace DesafioWarren.WebAPI.Services
{
    public interface ICustomerService
    {
        void criaCustomer(Customer customer);

        void atualizaCustomer(Customer customer);

        void excluiCustomer(Guid id);
    }
}
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DesafioWarren.WebAPI.Data;
using DesafioWarren.WebAPI.Models;
using DesafioWarren.WebAPI.Services;


namespace DesafioWarren.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(BancoCustomer.ListCustomer);
        }

        [HttpGet("byId/{id}")]
        public IActionResult GetById(Guid id)
        {
            var customerFound = BancoCustomer.ListCustomer.FirstOrDefault(a => a.Id.Equals(id));
            return customerFound == null ? BadRequest("Customer com o id " + id + " não foi encontrado.") : Ok(customerFound);
        }

        [HttpGet("byName")]
        public IActionResult GetByName(string fullName)
        {
            var customerFound = BancoCustomer.ListCustomer.FirstOrDefault(a =>
                a.FullName.Contains(fullName));

            return customerFound == null ? BadRequest("Cliente com o nome: " + fullName + " não foi encontrado.") : Ok(customerFound);
        }

        [HttpPost]
        public IActionResult Post(Customer customer)
        {
            _customerService.criaCustomer(customer);
            
            return Created("~http://localhost:5027/api/Customer", customer);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, Customer customer)
        {
            _customerService.atualizaCustomer(customer);
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _customerService.excluiCustomer(id);
            return NoContent();
        }
    }

}
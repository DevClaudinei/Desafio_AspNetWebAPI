using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.Services;

namespace SmartSchool.WebAPI.Controllers
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
            return Ok(SmartContext.ListCustomer);            
        }

        [HttpGet("byId/{id}")]
        public IActionResult GetById(int id)
        {
            var customerFound = SmartContext.ListCustomer.FirstOrDefault(a => a.Id == id);
            return customerFound == null ? BadRequest("Customer com o id " + id + " não foi encontrado.") : Ok(customerFound);
        }

        [HttpGet("byName")]
        public IActionResult GetByName(string fullName)
        {

            var customerFound = SmartContext.ListCustomer.FirstOrDefault(a =>
                a.FullName.Contains(fullName));

            return customerFound == null ? BadRequest("Cliente com o nome: " + fullName + " não foi encontrado.") : Ok(customerFound);

        }

        [HttpPost]
        public IActionResult Post(Customer customer)
        {
            _customerService.criaCustomer(customer);
            Console.WriteLine(customer.Birthdate);
            return Created("~http://localhost:5027/api/Customer", customer);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Customer customer)
        {
            _customerService.atualizaCustomer(customer);
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _customerService.excluiCustomer(id);
            return NoContent();
        }
    }

}
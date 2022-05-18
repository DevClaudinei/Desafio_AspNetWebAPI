using System;
using Microsoft.AspNetCore.Mvc;
using DesafioWarren.WebAPI.Models;
using DesafioWarren.WebAPI.Services;

namespace DesafioWarren.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public IActionResult Post(Customer customer)
    {
        var CreatedCustomer = _customerService.CreateCustomer(customer);
        return CreatedCustomer
            ? BadRequest($"Não foi possível cadastrar este customer com o Email '[{customer.Email}]' pois este email já esta cadastrado para outro usuário.")
            : Created("~http://localhost:5208/api/Customer", customer);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var CustomersFound = _customerService.GetCustomers();
        return Ok(CustomersFound);
    }

    [HttpGet("byId/{id}")]
    public IActionResult GetById(Guid id)
    {
        var CustomerFoundId = _customerService.GetCustomerById(id);
        return CustomerFoundId is null
            ? NotFound($"Customer com o id [{id}] não foi encontrado.")
            : Ok(CustomerFoundId);
    }

    [HttpGet("byName")]
    public IActionResult GetByName(string fullName)
    {
        var CustomerFoundName = _customerService.GetCustomerByName(fullName);
        return CustomerFoundName is not null
            ? Ok(CustomerFoundName)
            : NotFound($"Cliente com o nome: [{fullName}] não foi encontrado.");
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, Customer customer)
    {
        customer.Id = id;
        var UpdatedCustomer = _customerService.UpdateCustomer(customer);
        return UpdatedCustomer is not null 
            ? Ok(UpdatedCustomer)
            : NotFound($"Não é possível realizar a atualização do customer com o ID [{customer.Id}], pois ele não existe.");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var ExcludedCustomerById = _customerService.ExcludeCustomer(id);
        return ExcludedCustomerById is true
            ? NoContent()
            : NotFound($"Não é possível realizar a exclusão do cliente com o ID [{id}], pois ele não existe.");
    }
}
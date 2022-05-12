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

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_customerService.ListCustomer);
    }

    [HttpGet("byName")]
    public IActionResult GetByName(string fullName)
    {
        var CustomerFoundName = _customerService.GetCustomerByName(fullName);
        return CustomerFoundName is null ?
            BadRequest("Cliente com o nome: " + fullName + " não foi encontrado.")
            : Ok(CustomerFoundName);
    }

    [HttpGet("byId/{id}")]
    public IActionResult GetById(Guid id)
    {
        var CustomerFoundId = _customerService.GetCustomerById(id);
        return CustomerFoundId is null ?
            BadRequest("Customer com o id " + id + " não foi encontrado.")
            : Ok(CustomerFoundId);
    }

    [HttpPost]
    public IActionResult Post(Customer customer)
    {
        _customerService.CreateCustomer(customer);
        return Created("~http://localhost:5027/api/Customer", customer);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, Customer customer)
    {
        customer.Id = id;
        var UpdatedCustomerById = _customerService.UpdateCustomer(customer);
        return UpdatedCustomerById is true ? 
            Ok(customer)
            : BadRequest("Não é possível realizar a atualização do ID informado, pois ele não existe.");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var ExcludedCustomerById = _customerService.ExcludeCustomer(id);
        return ExcludedCustomerById is true ? 
            NoContent()
            : BadRequest("Não é possível realizar a exclusão do cliente com o ID informado, pois ele não existe.");
    }
}
using System;
using AppServices;
using DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerAppService _appService;

    public CustomersController(ICustomerAppService appService)
    {
        _appService = appService;
    }

    [HttpPost]
    public IActionResult Post(Customer customer)
    {
        var createdCustomer = _appService.CreateCustomer(customer); 
        return createdCustomer.isValid
        ? Created("~http://localhost:5160/api/Customers", createdCustomer.message)
        : BadRequest(createdCustomer.message);      
    }

    [HttpGet]
    public IActionResult Get()
    {
        var CustomersFound = _appService.GetCustomers();
        return Ok(CustomersFound);
    }

    [HttpGet("byId/{id}")]
    public IActionResult GetById(Guid id)
    {
        var CustomerFoundId = _appService.GetCustomerById(id);
        return CustomerFoundId is false
            ? NotFound($"Customer com o id [{id}] não foi encontrado.")
            : Ok(CustomerFoundId);
    }

    [HttpGet("byName")]
    public IActionResult GetByName(string fullName)
    {
        var CustomerFoundName = _appService.GetCustomerByName(fullName);
        return CustomerFoundName is true
            ? Ok(CustomerFoundName)
            : NotFound($"Cliente com o nome: [{fullName}] não foi encontrado.");
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, Customer customer)
    {
        customer.Id = id;
        var UpdatedCustomer = _appService.UpdateCustomer(customer);
        return UpdatedCustomer is not null
            ? Ok()
            : NotFound($"Não é possível realizar a atualização do customer com o ID [{id}], pois ele não existe.");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var ExcludedCustomerById = _appService.ExcludeCustomer(id);
        return ExcludedCustomerById is true
            ? NoContent()
            : NotFound($"Não é possível realizar a exclusão do cliente com o ID [{id}], pois ele não existe.");
    }
}
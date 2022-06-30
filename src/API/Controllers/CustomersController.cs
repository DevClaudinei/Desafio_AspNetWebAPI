using System;
using Application.Models;
using AppServices.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerAppService _appService;

    public CustomersController(ICustomerAppService appService, IMapper mapper)
    {
        _appService = appService ?? throw new ArgumentNullException(nameof(appService));
    }

    [HttpPost]
    public IActionResult Post(CreateCustomerRequest createCustomerRequest)
    {
        var createdCustomer = _appService.Create(createCustomerRequest);
        return createdCustomer.isValid
            ? Created("~http://localhost:5160/api/Customers", createdCustomer.message)
            : BadRequest(createdCustomer.message);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var customersFound = _appService.Get();
        return Ok(customersFound);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var customerFoundId = _appService.GetCustomerById(id);
        return customerFoundId is null
            ? NotFound($"Customer para o id: {id} não foi encontrado.")
            : Ok(customerFoundId);
    }

    [HttpGet("name/{fullName}")]
    public IActionResult GetByName(string fullName)
    {
        var customerFoundName = _appService.GetCustomerByName(fullName);
        return customerFoundName is not null
            ? Ok(customerFoundName)
            : NotFound($"Cliente para o nome: {fullName} não foi encontrado.");
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateCustomerRequest updateCustomerRequest)
    {
        updateCustomerRequest.Id = id;
        var updatedCustomer = _appService.Update(updateCustomerRequest);
        return updatedCustomer.isValid
            ? Ok()
            : NotFound(updatedCustomer.message);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var excludedCustomerById = _appService.Delete(id);
        return excludedCustomerById
            ? NoContent()
            : NotFound($"Cliente não encontrado para o ID: {id}.");
    }
}
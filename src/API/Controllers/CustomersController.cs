using System;
using System.Linq;
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
    public IActionResult GetById(long id)
    {
        var customerFoundId = _appService.GetCustomerById(id);
        return customerFoundId is null
            ? NotFound($"Customer para o id: {id} não foi encontrado.")
            : Ok(customerFoundId);
    }

    [HttpGet("name/{fullName}")]
    public IActionResult GetAllByName(string fullName)
    {
        var customerFoundName = _appService.GetAllCustomerByName(fullName);
        return customerFoundName.Any()
            ? Ok(customerFoundName)
            : NotFound($"Cliente para o nome: {fullName} não foi encontrado.");
    }

    [HttpPut("{id}")]
    public IActionResult Put(long id, UpdateCustomerRequest updateCustomerRequest)
    {
        updateCustomerRequest.Id = id;
        var updatedCustomer = _appService.Update(updateCustomerRequest);
        return updatedCustomer.isValid
            ? Ok()
            : NotFound(updatedCustomer.message);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var excludedCustomerById = _appService.Delete(id);
        return excludedCustomerById.isValid
            ? NoContent()
            : NotFound(excludedCustomerById.message);
    }
}
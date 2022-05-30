using System;
using AppServices;
using AutoMapper;
using DomainModels;
using DomainModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerAppService _appService;
    private readonly IMapper _mapper;

    public CustomersController(ICustomerAppService appService, IMapper mapper)
    {
        _appService = appService;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Post(CustomerToCreate customerToCreate)
    {
        var createdCustomer = _appService.CreateCustomer(_mapper.Map<Customer>(customerToCreate));
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
        return CustomerFoundId is null
            ? NotFound($"Customer para o id [{id}] não foi encontrado.")
            : Ok(CustomerFoundId);
    }

    [HttpGet("byName")]
    public IActionResult GetByName(string fullName)
    {
        var CustomerFoundName = _appService.GetCustomerByName(fullName);
        return CustomerFoundName is not null
            ? Ok(CustomerFoundName)
            : NotFound($"Cliente para o nome: [{fullName}] não foi encontrado.");
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, CustomerToUpdate customerToUpdate)
    {
        customerToUpdate.Id = id;
        var UpdatedCustomer = _appService.UpdateCustomer(_mapper.Map<Customer>(customerToUpdate));
        return UpdatedCustomer is not null
            ? Ok()
            : NotFound($"Cliente não encontrado para o ID [{id}].");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var ExcludedCustomerById = _appService.ExcludeCustomer(id);
        return ExcludedCustomerById
            ? NoContent()
            : NotFound($"Cliente não encontrado para o ID [{id}].");
    }
}
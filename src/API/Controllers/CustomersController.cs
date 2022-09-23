using Application.Models;
using AppServices.Services;
using AutoMapper;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;

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
        try
        {
            var createdCustomer = _appService.Create(createCustomerRequest);
            return Created("~http://localhost:5160/api/Customers", createdCustomer);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
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
        try
        {
            var customerFoundId = _appService.GetCustomerById(id);
            return Ok(customerFoundId);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("name/{fullName}")]
    public IActionResult GetAllByName(string fullName)
    {
        try
        {
            var customerFoundName = _appService.GetAllCustomerByName(fullName);
            return Ok(customerFoundName);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(long id, UpdateCustomerRequest updateCustomerRequest)
    {
        try
        {
            _appService.Update(id, updateCustomerRequest);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        try
        {
            _appService.Delete(id);
            return NoContent();
        }
        catch (BadRequestException e)
        {
            return NotFound(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
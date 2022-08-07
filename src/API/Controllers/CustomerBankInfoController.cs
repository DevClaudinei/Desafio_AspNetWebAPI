﻿using Application.Models;
using AppServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerBankInfoController : ControllerBase
{
    private readonly ICustomerBankInfoAppService _appService;

    public CustomerBankInfoController(ICustomerBankInfoAppService appService)
    {
        _appService = appService ?? throw new System.ArgumentNullException(nameof(appService));
    }

    // POST api/<CustomerBankInfoController>
    [HttpPost]
    public IActionResult Post(CreateCustomerBankInfoRequest createCustomerBackInfoRequest)
    {
        var createdCustomerBackInfo = _appService.Create(createCustomerBackInfoRequest);
        return createdCustomerBackInfo.isValid
            ? Created("~http://localhost:5160/api/CustomerBankInfo", createdCustomerBackInfo.message)
            : BadRequest(createdCustomerBackInfo.message);
    }
    
    // GET: api/<CustomerBankInfoController>
    [HttpGet]
    public IActionResult Get()
    {
        var customersBankInfoFound = _appService.Get();
        return Ok(customersBankInfoFound);
    }
    
    // GET api/<CustomerBankInfoController>/5
    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var customersBankInfoFound = _appService.GetCustomerBankInfoById(id);
        return customersBankInfoFound is null
            ? NotFound($"CustomerBankInfo para o id {id} não foi encontrado.")
            : Ok(customersBankInfoFound);
    }

    [HttpGet("name/{account}")]
    public IActionResult GetAllByName(string account)
    {
        var customerFoundName = _appService.GetAllCustomerBackInfoByAccount(account);
        return customerFoundName is not null
            ? Ok(customerFoundName)
            : NotFound($"Cliente para o nome: {account} não foi encontrado.");
    }

    // PUT api/<CustomerBankInfoController>/5
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        updateCustomerBankInfoRequest.Id = id;
        var updatedCustomerBankInfo = _appService.Update(updateCustomerBankInfoRequest);
        return updatedCustomerBankInfo.isValid
            ? Ok()
            : NotFound(updatedCustomerBankInfo.message);
    }

    // DELETE api/<CustomerBankInfoController>/5
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var excludedCustomerBankInfoById = _appService.Delete(id);
        return excludedCustomerBankInfoById
            ? NoContent()
            : NotFound($"Cliente não encontrado para o ID: {id}.");
    }
}
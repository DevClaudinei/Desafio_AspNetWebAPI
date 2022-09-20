using Application.Models;
using AppServices.Services.Interfaces;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerBankInfosController : ControllerBase
{
    private readonly ICustomerBankInfoAppService _customerBankInfoAppService;

    public CustomerBankInfosController(ICustomerBankInfoAppService appService)
    {
        _customerBankInfoAppService = appService ?? throw new System.ArgumentNullException(nameof(appService));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var customersBankInfoFound = _customerBankInfoAppService.GetAllCustomerBankInfo();
        return Ok(customersBankInfoFound);
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        try
        {
            var customersBankInfoFound = _customerBankInfoAppService.Get(id);
            return Ok(customersBankInfoFound);
        }
        catch (GenericNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("account/{account}")]
    public IActionResult GetByAccount(string account)
    {
        try
        {
            var customerFoundName = _customerBankInfoAppService.GetAllCustomerBackInfoByAccount(account);
            return Ok(customerFoundName);
        }
        catch (GenericNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPatch("{id}/deposit")]
    public IActionResult DepositMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        try
        {
            _customerBankInfoAppService.DepositMoney(id, updateCustomerBankInfoRequest);
            return Ok();
        }
        catch (GenericBalancesException e)
        {
            return BadRequest(e.Message);
        }
        catch (GenericNotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id}/withdraw")]
    public IActionResult WithdrawMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        try
        {
            _customerBankInfoAppService.WithdrawMoney(id, updateCustomerBankInfoRequest);
            return Ok();
        }
        catch (GenericBalancesException e)
        {
            return BadRequest(e.Message);
        }
        catch (GenericNotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
}
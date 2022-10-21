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
        var customersBankInfoFound = _customerBankInfoAppService.GetAll();
        return Ok(customersBankInfoFound);
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        try
        {
            var customersBankInfoFound = _customerBankInfoAppService.GetById(id);
            return Ok(customersBankInfoFound);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("account/{account}")]
    public IActionResult GetByAccount(string account)
    {
        try
        {
            var customerFoundName = _customerBankInfoAppService.GetByAccount(account);
            return Ok(customerFoundName);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPatch("{id}/deposit/{amount}")]
    public IActionResult Deposit(long id, decimal amount)
    {
        try
        {
            _customerBankInfoAppService.Deposit(id, amount);
            return Ok();
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id}/withdraw/{amount}")]
    public IActionResult Withdraw(long id, decimal amount)
    {
        try
        {
            _customerBankInfoAppService.Withdraw(id, amount);
            return Ok();
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
}
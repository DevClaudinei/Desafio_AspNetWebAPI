using Application.Models;
using AppServices.Services.Interfaces;
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
        var customersBankInfoFound = _customerBankInfoAppService.GetCustomerBankInfoById(id);
        return customersBankInfoFound is null
            ? NotFound($"CustomerBankInfo para o id: {id} não foi encontrado.")
            : Ok(customersBankInfoFound);
    }

    [HttpGet("account/{account}")]
    public IActionResult GetByAccount(string account)
    {
        var customerFoundName = _customerBankInfoAppService.GetAllCustomerBackInfoByAccount(account);
        return customerFoundName is not null
            ? Ok(customerFoundName)
            : NotFound($"CustomerBankInfo para account: {account} não foi encontrada.");
    }

    [HttpPut("DepositMoney/{id}")]
    public IActionResult DepositMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        updateCustomerBankInfoRequest.Id = id;
        var updatedCustomerBankInfo = _customerBankInfoAppService.DepositMoney(updateCustomerBankInfoRequest);
        return updatedCustomerBankInfo.isValid
            ? Ok()
            : NotFound(updatedCustomerBankInfo.message);
    }

    [HttpPut("WithdrawMoney/{id}")]
    public IActionResult WithdrawMoney(long id, UpdateCustomerBankInfoRequest updateCustomerBankInfoRequest)
    {
        updateCustomerBankInfoRequest.Id = id;
        var updatedCustomerBankInfo = _customerBankInfoAppService.WithdrawMoney(updateCustomerBankInfoRequest);
        return updatedCustomerBankInfo.isValid
            ? Ok()
            : NotFound(updatedCustomerBankInfo.message);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var excludedCustomerBankInfoById = _customerBankInfoAppService.Delete(id);
        return excludedCustomerBankInfoById.isValid
            ? NoContent()
            : NotFound(excludedCustomerBankInfoById.message);
    }
}
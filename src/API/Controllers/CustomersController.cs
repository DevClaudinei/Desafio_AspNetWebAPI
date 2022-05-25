using AppServices;
using DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace OnionArchitecture.WebAPI.Controllers;

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
        var CreatedCustomer = _appService.CreateCustomer(customer);
        return CreatedCustomer
            ? Created("~http://localhost:5160/api/Customers", customer)
            : BadRequest($"Não foi possível cadastrar este customer. Email '[{customer.Email}]' ou CPF '[{customer.Cpf}] já esta cadastrado para outro usuário.");
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
            ? NotFound($"Customer com o id [{id}] não foi encontrado.")
            : Ok(CustomerFoundId);
    }

    [HttpGet("byName")]
    public IActionResult GetByName(string fullName)
    {
        var CustomerFoundName = _appService.GetCustomerByName(fullName);
        return CustomerFoundName is not null
            ? Ok(CustomerFoundName)
            : NotFound($"Cliente com o nome: [{fullName}] não foi encontrado.");
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, Customer customer)
    {
        customer.Id = id;
        var UpdatedCustomer = _appService.UpdateCustomer(customer);
        return UpdatedCustomer is not null
            ? Ok(UpdatedCustomer)
            : NotFound($"Não é possível realizar a atualização do customer com o ID [{customer.Id}], pois ele não existe.");
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

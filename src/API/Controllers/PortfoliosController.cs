using Application.Models.Portfolio.Request;
using Application.Models.PortfolioProduct;
using AppServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfoliosController : Controller
{
    private readonly IPortfolioAppService _portfolioAppService;

    public PortfoliosController(IPortfolioAppService appService)
    {
        _portfolioAppService = appService ?? throw new System.ArgumentNullException(nameof(appService));
    }

    [HttpPost]
    public IActionResult Post(CreatePortfolioRequest portfolioCreate)
    {
        var createdPortfolio = _portfolioAppService.CreatePortfolio(portfolioCreate);
        return createdPortfolio.isValid
            ? Created("~http://localhost:5160/api/Customers", createdPortfolio.message)
            : BadRequest(createdPortfolio.message);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var portfoliosFound = _portfolioAppService.GetAllPortfolios();
        return Ok(portfoliosFound);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var portfolioFound = _portfolioAppService.GetPortfolioById(id);
        return portfolioFound is null
            ? NotFound($"Portfolio para o Id: {id} não foi encontrado.")
            : Ok(portfolioFound);
    }

    [HttpGet("/totalBalance")]
    public IActionResult GetByTotalBalance(Guid portfolioId)
    {
        var portfolioBalance = _portfolioAppService.GetTotalBalance(portfolioId);
        return portfolioBalance.isValid
            ? Ok(portfolioBalance.message)
            : BadRequest(portfolioBalance.message);
    }

    [HttpPut("AddProduct")]
    public IActionResult AddProduct(UpdatePortfolioProductRequest updatePortfolioProductRequest)
    {
        var productsToAddOnPortfolio = _portfolioAppService.AddProduct(updatePortfolioProductRequest);
        return productsToAddOnPortfolio.isValid
            ? Ok()
            : NotFound(productsToAddOnPortfolio.message);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var excludedPortfolioById = _portfolioAppService.Delete(id);
        return excludedPortfolioById
            ? NoContent()
            : NotFound($"Portfolio não encontrado para o ID: {id}.");
    }
}

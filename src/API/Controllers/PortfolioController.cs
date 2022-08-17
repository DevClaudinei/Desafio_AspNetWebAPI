using Application.Models.Portfolio.Request;
using AppServices.Services;
using AppServices.Services.Interfaces;
using DomainModels.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : Controller
{
    private readonly IPortfolioAppService _appService;

    public PortfolioController(IPortfolioAppService appService)
    {
        _appService = appService ?? throw new System.ArgumentNullException(nameof(appService));
    }

    [HttpPost]
    public IActionResult Post(PortfolioCreate portfolioCreate)
    {
        var createdPortfolio = _appService.CreatePortfolio(portfolioCreate);
        return createdPortfolio.isValid
            ? Created("~http://localhost:5160/api/Customers", createdPortfolio.message)
            : BadRequest(createdPortfolio.message);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var portfoliosFound = _appService.GetAllPortfolios();
        return Ok(portfoliosFound);
    }

    [HttpGet("/totalBalance")]
    public IActionResult GetByTotalBalance(Guid portfolioId)
    {
        var portfolioBalance = _appService.GetTotalBalance(portfolioId);
        return Ok(portfolioBalance);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var excludedPortfolioById = _appService.Delete(id);
        return excludedPortfolioById
            ? NoContent()
            : NotFound($"Portfolio não encontrado para o ID: {id}.");
    }
}

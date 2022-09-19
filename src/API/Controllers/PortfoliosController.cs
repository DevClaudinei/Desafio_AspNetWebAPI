﻿using Application.Models.Portfolio.Request;
using AppServices.Services.Interfaces;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Mvc;

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
        try
        {
            var createdPortfolio = _portfolioAppService.CreatePortfolio(portfolioCreate);
            return Created("~http://localhost:5160/api/Customers", createdPortfolio);
        }
        catch (GenericNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch(GenericResourceAlreadyExistsException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("/investment")]
    public IActionResult Post(CreateInvestmentRequest investmentRequest, long customerBankId)
    {
        try
        {
            var investmentCustomer = _portfolioAppService.Invest(investmentRequest, customerBankId);
            return Created("~http://localhost:5160/api/PortfolioProducts", investmentCustomer);
        }
        catch (GenericNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (GenericBalancesException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public IActionResult Get()
    {
        var portfoliosFound = _portfolioAppService.GetAllPortfolios();
        return Ok(portfoliosFound);
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        try
        {
            var portfolioFound = _portfolioAppService.GetPortfolioById(id);
            return Ok(portfolioFound);
        }
        catch (GenericNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("/total-balance")]
    public IActionResult GetByTotalBalance(long portfolioId)
    {
        try
        {
            var portfolioBalance = _portfolioAppService.GetTotalBalance(portfolioId);
            return Ok(portfolioBalance);
        }
        catch (GenericBalancesException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        try
        {
            _portfolioAppService.Delete(id);
            return NoContent();
        }
        catch (GenericNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
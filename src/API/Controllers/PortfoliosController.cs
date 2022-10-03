using Application.Models.Enum;
using Application.Models.Portfolio.Request;
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
            var createdPortfolio = _portfolioAppService.Create(portfolioCreate);
            return Created("~http://localhost:5160/api/Customers", createdPortfolio);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch(BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{orderDirection}/investment")]
    public IActionResult Post(InvestmentRequest investmentRequest, OrderDirection orderDirection)
    {
        try
        {
            var investmentCustomer = _portfolioAppService.Invest(investmentRequest, orderDirection);
            return Created("~http://localhost:5160/api/PortfolioProducts", investmentCustomer);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public IActionResult Get()
    {
        var portfoliosFound = _portfolioAppService.GetAll();
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
        catch (NotFoundException e)
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
        catch (NotFoundException e)
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
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
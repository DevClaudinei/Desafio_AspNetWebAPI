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
            return NotFound(e.Message);
        }
        catch(BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("invest")]
    public IActionResult Post(InvestmentRequest investmentRequest)
    {
        try
        {
            var investmentCustomer = _portfolioAppService.Invest(investmentRequest);
            return Created("~http://localhost:5160/api/PortfolioProducts", investmentCustomer);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("uninvest")]
    public IActionResult Post(UninvestimentRequest uninvestimentRequest)
    {
        try
        {
            var investmentCustomer = _portfolioAppService.Uninvest(uninvestimentRequest);
            return Created("~http://localhost:5160/api/PortfolioProducts", investmentCustomer);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
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

    [HttpGet("total-balance/{id}")]
    public IActionResult GetByTotalBalance(long id)
    {
        try
        {
            var portfolioBalance = _portfolioAppService.GetTotalBalance(id);
            return Ok(portfolioBalance);
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
            _portfolioAppService.Delete(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
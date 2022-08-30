using Application.Models.Portfolio.Request;
using AppServices.Services.Interfaces;
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
    public IActionResult Get(long id)
    {
        var portfolioFound = _portfolioAppService.GetPortfolioById(id);
        return portfolioFound is null
            ? NotFound($"Portfolio para o Id: {id} não foi encontrado.")
            : Ok(portfolioFound);
    }

    [HttpGet("/totalBalance")]
    public IActionResult GetByTotalBalance(long portfolioId)
    {
        var portfolioBalance = _portfolioAppService.GetTotalBalance(portfolioId);
        return portfolioBalance.isValid
            ? Ok(portfolioBalance.message)
            : BadRequest(portfolioBalance.message);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var excludedPortfolioById = _portfolioAppService.Delete(id);
        return excludedPortfolioById
            ? NoContent()
            : NotFound($"Portfolio não encontrado para o ID: {id}.");
    }
}

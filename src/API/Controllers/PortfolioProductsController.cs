using AppServices.Services.Interfaces;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioProductsController : Controller
{
    private readonly IPortfolioProductAppService _portfolioProductAppService;

    public PortfolioProductsController(IPortfolioProductAppService portfolioProductAppService)
    {
        _portfolioProductAppService = portfolioProductAppService ?? throw new System.ArgumentNullException(nameof(portfolioProductAppService));
    }

    [HttpGet]
    public IActionResult Get()
    {
        var portfoliosProduct = _portfolioProductAppService.GetAll();
        return Ok(portfoliosProduct);
    }

    [HttpGet("{portfolioId}/{productId}")]
    public IActionResult Get(long portfolioId, long productId)
    {
        try
        {
            var portfoliosProduct = _portfolioProductAppService.GetById(portfolioId, productId);
            return Ok(portfoliosProduct);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
}

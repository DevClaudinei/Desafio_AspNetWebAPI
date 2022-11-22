using Application.Models.Product.Request;
using AppServices.Services.Interfaces;
using DomainServices.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductAppService _productAppService;

    public ProductsController(IProductAppService appService)
    {
        _productAppService = appService ?? throw new System.ArgumentNullException(nameof(appService));
    }

    [HttpPost]
    public IActionResult Post(CreateProductRequest createProductRequest)
    {
        try
        {
            var productCreated = _productAppService.Create(createProductRequest);
            return Created("~http://localhost:5160/api/Product", productCreated);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var productsFound = _productAppService.GetAll();
            return Ok(productsFound);
        }
        catch
        {
            return NoContent();
        }
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        try
        {
            var productFound = _productAppService.GetById(id);
            return Ok(productFound);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("symbol/{symbol}")]
    public IActionResult Get(string symbol)
    {
        try
        {
            var productsFound = _productAppService.GetBySymbol(symbol);
            return Ok(productsFound);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(long id, UpdateProductRequest updateProductRequest)
    {
        try
        {
            _productAppService.Update(id, updateProductRequest);
            return Ok();
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
            _productAppService.Delete(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
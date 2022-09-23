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
        var productsFound = _productAppService.GetAllProducts();
        return Ok(productsFound);
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        try
        {
            var productFound = _productAppService.GetProductById(id);
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
            var productsFound = _productAppService.GetAllProductBySymbol(symbol);
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
            updateProductRequest.Id = id;
            _productAppService.Update(updateProductRequest);
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
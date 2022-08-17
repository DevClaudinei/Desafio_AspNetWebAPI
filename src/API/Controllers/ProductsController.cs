using Application.Models.Product.Request;
using AppServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductAppService _appService;

    public ProductsController(IProductAppService appService)
    {
        _appService = appService ?? throw new System.ArgumentNullException(nameof(appService));
    }

    [HttpPost]
    public IActionResult Post(CreateProductRequest createProductRequest)
    {
        var productCreated = _appService.Create(createProductRequest);
        return productCreated.isValid
            ? Created("~http://localhost:5160/api/Product", productCreated.message)
            : BadRequest(productCreated.message);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var productsFound = _appService.GetAllProducts();
        return Ok(productsFound);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var productFound = _appService.GetProductById(id);
        return productFound is null
            ? NotFound($"Product para o id {id} não foi encontrado.")
            : Ok(productFound);
    }

    [HttpGet("symbol/{symbol}")]
    public IActionResult Get(string symbol)
    {
        var productsFound = _appService.GetAllProductBySymbol(symbol);
        return productsFound is null
            ? NotFound($"Product para o symbol {symbol} não foi encontrado.")
            : Ok(productsFound);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateProductRequest updateProductRequest)
    {
        updateProductRequest.Id = id;
        var updatedCustomerBankInfo = _appService.Update(updateProductRequest);
        return updatedCustomerBankInfo.isValid
            ? Ok()
            : NotFound(updatedCustomerBankInfo.message);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var excludedProductById = _appService.Delete(id);
        return excludedProductById
            ? NoContent()
            : NotFound($"Product não encontrado para o ID: {id}.");
    }
}

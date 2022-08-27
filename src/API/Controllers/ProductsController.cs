using Application.Models.Product.Request;
using AppServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

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
        var productCreated = _productAppService.Create(createProductRequest);
        return productCreated.isValid
            ? Created("~http://localhost:5160/api/Product", productCreated.message)
            : BadRequest(productCreated.message);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var productsFound = _productAppService.GetAllProducts();
        return Ok(productsFound);
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var productFound = _productAppService.GetProductById(id);
        return productFound is null
            ? NotFound($"Product para o id {id} não foi encontrado.")
            : Ok(productFound);
    }

    [HttpGet("symbol/{symbol}")]
    public IActionResult Get(string symbol)
    {
        var productsFound = _productAppService.GetAllProductBySymbol(symbol);
        return productsFound is null
            ? NotFound($"Product para o symbol {symbol} não foi encontrado.")
            : Ok(productsFound);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateProductRequest updateProductRequest)
    {
        updateProductRequest.Id = id;
        var updatedCustomerBankInfo = _productAppService.Update(updateProductRequest);
        return updatedCustomerBankInfo.isValid
            ? Ok()
            : NotFound(updatedCustomerBankInfo.message);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var excludedProductById = _productAppService.Delete(id);
        return excludedProductById
            ? NoContent()
            : NotFound($"Product não encontrado para o ID: {id}.");
    }
}

using Application.Models.Product.Request;
using Application.Models.Product.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class ProductAppService : IProductAppService
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductAppService(IProductService productService, IMapper mapper)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public (bool isValid, string message) Create(CreateProductRequest createProductRequest)
    {
        var product = _mapper.Map<Product>(createProductRequest);
        var createdProduct = _productService.CreateProduct(product);

        if (createdProduct.isValid) return (true, createdProduct.message);

        return (false, createdProduct.message);
    }

    public IEnumerable<ProductResult> GetAllProducts()
    {
        var product = _productService.GetAllProducts();
        return _mapper.Map<IEnumerable<ProductResult>>(product);
    }

    public ProductResult GetAllProductBySymbol(string symbol)
    {
        var product = _productService.GetAllProducsBySymbol(symbol);
        if (product is null) return null;

        return _mapper.Map<ProductResult>(product);
    }

    public ProductResult GetProductById(long id)
    {
        var product = _productService.GetProductById(id);
        if (product is null) return null;

        return _mapper.Map<ProductResult>(product);
    }

    public (bool isValid, string message) Update(UpdateProductRequest updateProductRequest)
    {
        var productToUpdate = _mapper.Map<Product>(updateProductRequest);
        return _productService.UpdateProduct(productToUpdate);
    }

    public bool Delete(long id)
    {
        var deletedProduct = _productService.Delete(id);
        return deletedProduct;
    }
}

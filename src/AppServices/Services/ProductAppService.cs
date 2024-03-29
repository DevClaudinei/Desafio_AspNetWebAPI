﻿using Application.Models.Product.Request;
using Application.Models.Product.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class ProductAppService : IProductAppService
{
    private readonly IMapper _mapper;
    private readonly IProductService _productService;

    public ProductAppService(IProductService productService, IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    public long Create(CreateProductRequest createProductRequest)
    {
        var product = _mapper.Map<Product>(createProductRequest);
        return _productService.Create(product);
    }

    public IEnumerable<ProductResult> GetAll()
    {
        var product = _productService.GetAll();
        return _mapper.Map<IEnumerable<ProductResult>>(product);
    }

    public ProductResult GetBySymbol(string symbol)
    {
        var product = _productService.GetBySymbol(symbol)
            ?? throw new NotFoundException($"Product for the symbol: {symbol} could not be found.");

        return _mapper.Map<ProductResult>(product);
    }

    public ProductResult GetById(long id)
    {
        var product = _productService.GetById(id)
            ?? throw new NotFoundException($"Product for the Id: {id} could not be found.");

        return _mapper.Map<ProductResult>(product);
    }

    public Product Get(long id)
    {
        var product = _productService.GetById(id)
            ?? throw new NotFoundException($"Product for the Id: {id} could not be found.");

        return product;
    }

    public void Update(long id, UpdateProductRequest updateProductRequest)
    {
        updateProductRequest.Id = id;
        var productToUpdate = _mapper.Map<Product>(updateProductRequest);
        _productService.Update(productToUpdate);
    }

    public void Delete(long id)
    {
        _productService.Delete(id);
    }
}
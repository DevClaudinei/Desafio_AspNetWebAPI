using Application.Models.Product.Request;
using Application.Models.Product.Response;
using System;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IProductAppService
{
    (bool isValid, string message) Create(CreateProductRequest createProductRequest);
    IEnumerable<ProductResult> GetAllProducts();
    ProductResult GetProductById(Guid id);
    ProductResult GetAllProductBySymbol(string symbol);
    (bool isValid, string message) Update(UpdateProductRequest updateProductRequest);
    bool Delete(Guid id);
}

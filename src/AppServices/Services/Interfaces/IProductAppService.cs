using Application.Models.Product.Request;
using Application.Models.Product.Response;
using DomainModels.Entities;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IProductAppService
{
    long Create(CreateProductRequest createProductRequest);
    IEnumerable<ProductResult> GetAll();
    ProductResult GetById(long id);
    ProductResult GetBySymbol(string symbol);
    void Update(long id, UpdateProductRequest updateProductRequest);
    void Delete(long id);
    public Product Get(long id);
}
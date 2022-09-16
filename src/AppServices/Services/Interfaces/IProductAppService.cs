using Application.Models.Product.Request;
using Application.Models.Product.Response;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IProductAppService
{
    long Create(CreateProductRequest createProductRequest);
    IEnumerable<ProductResult> GetAllProducts();
    ProductResult GetProductById(long id);
    ProductResult GetAllProductBySymbol(string symbol);
    void Update(UpdateProductRequest updateProductRequest);
    void Delete(long id);
}
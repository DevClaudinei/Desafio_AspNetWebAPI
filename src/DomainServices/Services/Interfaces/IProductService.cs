using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IProductService
{
    long CreateProduct(Product product);
    IEnumerable<Product> GetAllProducts();
    Product GetProductById(long id);
    Product GetAllProducsBySymbol(string symbol);
    void UpdateProduct(Product product);
    void Delete(long id);
}
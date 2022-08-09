using DomainModels.Entities;
using System;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IProductService
{
    (bool isValid, string message) CreateProduct(Product product);
    IEnumerable<Product> GetAllProducts();
    Product GetProductById(Guid id);
    Product GetAllProducsBySymbol(string symbol);
    (bool isValid, string message) UpdateProduct(Product product);
    bool Delete(Guid id);
}

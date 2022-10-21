using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IProductService
{
    long Create(Product product);
    IEnumerable<Product> GetAll();
    Product GetById(long id);
    Product GetBySymbol(string symbol);
    void Update(Product product);
    void Delete(long id);
}
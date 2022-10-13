using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DomainServices.Services;

public class ProductService : BaseService, IProductService
{
    private readonly IRepository<Product> _productService;

    public ProductService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory)
    {
        _productService = repositoryFactory.Repository<Product>();
    }

    public long Create(Product product)
    {
        var repository = UnitOfWork.Repository<Product>();
        var exists = Exists(x => x.Symbol.Equals(product.Symbol));
        
        if (exists) throw new BadRequestException($"Product: {product.Symbol} are already registered");

        repository.Add(product);
        UnitOfWork.SaveChanges();

        return product.Id;
    }

    private bool Exists(Expression<Func<Product, bool>> predicate)
    {
        return _productService.Any(predicate);
    }

    public IEnumerable<Product> GetAll()
    {
        var query = _productService.MultipleResultQuery();

        return _productService.Search(query);
    }

    public Product GetBySymbol(string symbol)
    {
        var query = _productService.SingleResultQuery()
            .AndFilter(x => x.Symbol.Equals(symbol));

        return _productService.SingleOrDefault(query);
    }

    public Product GetById(long id)
    {
        var query = _productService.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return _productService.SingleOrDefault(query);
    }

    public void Update(Product product)
    {
        var repository = UnitOfWork.Repository<Product>();
        Exists(x => x.Id.Equals(product.Id));

        repository.Update(product);
        UnitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var repository = UnitOfWork.Repository<Product>();
        var exists = Exists(x => x.Id.Equals(id));

        if (!exists) throw new NotFoundException($"Product not found for id: {id}.");

        repository.Remove(x => x.Id.Equals(id));
    }
}
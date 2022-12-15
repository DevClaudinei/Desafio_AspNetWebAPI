using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DomainServices.Services;

public class ProductService : BaseService, IProductService
{
    public ProductService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory) { }

    public long Create(Product product)
    {
        var unitOfWork = UnitOfWork.Repository<Product>();
        
        if (Exists(x => x.Symbol.Equals(product.Symbol)))
            throw new BadRequestException($"Product: {product.Symbol} already exists");

        unitOfWork.Add(product);
        UnitOfWork.SaveChanges();

        return product.Id;
    }

    private bool Exists(Expression<Func<Product, bool>> predicate)
    {
        var repository = RepositoryFactory.Repository<Product>();
        return repository.Any(predicate);
    }

    public IEnumerable<Product> GetAll()
    {
        var repository = RepositoryFactory.Repository<Product>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public Product GetBySymbol(string symbol)
    {
        var repository = RepositoryFactory.Repository<Product>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Symbol.Equals(symbol));

        return repository.SingleOrDefault(query);
    }

    public Product GetById(long id)
    {
        var repository = RepositoryFactory.Repository<Product>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(query);
    }

    public void Update(Product product)
    {
        var unitOfWork = UnitOfWork.Repository<Product>();

        if (!Exists(x => x.Id.Equals(product.Id))) 
            throw new NotFoundException($"Product not found for id: {product.Id}.");

        unitOfWork.Update(product);
        UnitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var unitOfWork = UnitOfWork.Repository<Product>();
        var exists = Exists(x => x.Id.Equals(id));

        if (!exists) throw new NotFoundException($"Product not found for id: {id}.");

        unitOfWork.Remove(x => x.Id.Equals(id));
    }
}
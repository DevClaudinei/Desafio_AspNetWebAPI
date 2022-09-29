using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.Repository.Extensions;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DomainServices.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public ProductService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public long Create(Product product)
    {
        var repository = _unitOfWork.Repository<Product>();
        var exists = Exists(x => x.Symbol.Equals(product.Symbol));
        
        if (exists) throw new BadRequestException($"Product: {product.Symbol} are already registered");

        repository.Add(product);
        _unitOfWork.SaveChanges();

        return product.Id;
    }

    private bool Exists(Expression<Func<Product, bool>> predicate)
    {
        var repository = _repositoryFactory.Repository<Product>();
        return repository.Any(predicate);
    }

    public IEnumerable<Product> GetAll()
    {
        var repository = _repositoryFactory.Repository<Product>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public Product GetBySymbol(string symbol)
    {
        var repository = _repositoryFactory.Repository<Product>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Symbol.Equals(symbol));

        return repository.SingleOrDefault(query);
    }

    public Product GetById(long id)
    {
        var repository = _repositoryFactory.Repository<Product>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(query);
    }

    public void Update(Product product)
    {
        var repository = _unitOfWork.Repository<Product>();
        Exists(x => x.Id.Equals(product.Id));

        repository.Update(product);
        _unitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var repository = _unitOfWork.Repository<Product>();
        var exists = Exists(x => x.Id.Equals(id));

        if (!exists) throw new NotFoundException($"Product not found for id: {id}.");

        repository.Remove(x => x.Id.Equals(id));
    }
}
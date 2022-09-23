using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;

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
        Exists(product);

        repository.Add(product);
        _unitOfWork.SaveChanges();

        return product.Id;
    }

    private bool Exists(Product product)
    {
        var repository = _repositoryFactory.Repository<Product>();

        if (repository.Any(x => x.Symbol.Equals(product.Symbol)))
            throw new BadRequestException($"Product: {product.Symbol} is already registered");

        return true;
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
        var updatedProduct = GetById(product.Id);
        if (updatedProduct is null) throw new NotFoundException($"Product not found for id: {product.Id}.");

        var repository = _unitOfWork.Repository<Product>();
        updatedProduct.UnitPrice = product.UnitPrice;

        repository.Update(product);
        _unitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var repository = _unitOfWork.Repository<Product>();
        var productToDelete = GetById(id);

        if (productToDelete is null) throw new NotFoundException($"Product not found for id: {id}.");

        repository.Remove(x => x.Id.Equals(id));
    }
}
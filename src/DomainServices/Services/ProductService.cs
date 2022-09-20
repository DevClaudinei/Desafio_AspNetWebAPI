using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;

namespace DomainServices.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public ProductService(IUnitOfWork unitOfWork, IRepositoryFactory repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public long CreateProduct(Product product)
    {
        var repository = _unitOfWork.Repository<Product>();
        CheckIfProductAlreadyExists(product);

        repository.Add(product);
        _unitOfWork.SaveChanges();

        return product.Id;
    }

    private bool CheckIfProductAlreadyExists(Product product)
    {
        var repository = _repositoryFactory.Repository<Product>();

        if (repository.Any(x => x.Symbol.Equals(product.Symbol)))
            throw new GenericResourceAlreadyExistsException($"Product: {product.Symbol} is already registered");

        return true;
    }

    public IEnumerable<Product> GetAllProducts()
    {
        var repository = _repositoryFactory.Repository<Product>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public Product GetAllProducsBySymbol(string symbol)
    {
        var repository = _repositoryFactory.Repository<Product>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Symbol.Equals(symbol));

        return repository.SingleOrDefault(query);
    }

    public Product GetProductById(long id)
    {
        var repository = _repositoryFactory.Repository<Product>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(query);
    }

    public void UpdateProduct(Product product)
    {
        var updatedProduct = GetProductById(product.Id);
        if (updatedProduct is null) throw new GenericNotFoundException($"Product not found for id: {product.Id}.");

        var repository = _unitOfWork.Repository<Product>();
        updatedProduct.UnitPrice = product.UnitPrice;

        repository.Update(product);
        _unitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var repository = _unitOfWork.Repository<Product>();
        var productToDelete = GetProductById(id);

        if (productToDelete is null) throw new GenericNotFoundException($"Product not found for id: {id}.");

        repository.Remove(x => x.Id.Equals(id));
    }
}
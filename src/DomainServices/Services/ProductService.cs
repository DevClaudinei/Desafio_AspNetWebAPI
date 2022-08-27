using DomainModels.Entities;
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

    public ProductService(IUnitOfWork unitOfWork, IRepositoryFactory repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public (bool isValid, string message) CreateProduct(Product product)
    {
        var repository = _unitOfWork.Repository<Product>();
        var productAlredyExists = CheckIfProductAlreadyExists(product);

        if (productAlredyExists.exists is true) return (false, productAlredyExists.errorMessage);

        repository.Add(product);
        _unitOfWork.SaveChanges();

        return (true, product.Id.ToString());
    }

    private (bool exists, string errorMessage) CheckIfProductAlreadyExists(Product product)
    {
        var messageTemplate = "The {0}: {1} is already registered";
        var repository = _repositoryFactory.Repository<Product>();

        if (repository.Any(x => x.Symbol.Equals(product.Symbol)))
        {
            return (true, string.Format(messageTemplate, "Product", product.Symbol));
        }

        return default;
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

    public Product GetProductById(Guid id)
    {
        var repository = _repositoryFactory.Repository<Product>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(query);
    }

    public (bool isValid, string message) UpdateProduct(Product product)
    {
        var updatedProduct = GetProductById(product.Id);
        if (updatedProduct is null) return (false, $"Cliente não encontrado para o Id: {product.Id}.");

        //product.ConvertedAt = updatedProduct.ConvertedAt;
        var repository = _unitOfWork.Repository<Product>();
        updatedProduct.UnitPrice = product.UnitPrice;
        
        repository.Update(product);
        _unitOfWork.SaveChanges();

        return (true, product.Id.ToString());
    }

    public bool Delete(Guid id)
    {
        var repository = _unitOfWork.Repository<Product>();

        return repository.Remove(x => x.Id.Equals(id)) > 0;
    }
}

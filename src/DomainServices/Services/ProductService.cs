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

    public ProductService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public (bool isValid, string message) CreateProduct(Product product)
    {
        //product.NetValue = product.Quotes * product.UnitPrice;
        var repository = _unitOfWork.Repository<Product>();

        repository.Add(product);
        _unitOfWork.SaveChanges();

        return (true, product.Id.ToString());
    }

    private (bool exists, string errorMessage) VerifyCustomerBankInfoAlreadyExists(CustomerBankInfo customerBankInfo)
    {
        var messageTemplate = "The {0}: {1} is already linked to a|an {2}";
        var repository = _repositoryFactory.Repository<CustomerBankInfo>();

        if (repository.Any(x => x.Account.Equals(customerBankInfo.Account)))
        {
            return (true, string.Format(messageTemplate, "Account", customerBankInfo.Account, "Customer"));
        }

        if (repository.Any(x => x.Customer.Id.Equals(customerBankInfo.CustomerId)))
        {
            return (true, string.Format(messageTemplate, "CustomerId", customerBankInfo.CustomerId, "Account"));
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
        product.ConvertedAt = updatedProduct.ConvertedAt;
        //product.NetValue = product.Quotes * product.UnitPrice;

        if (updatedProduct is null) return (false, $"Cliente não encontrado para o Id: {product.Id}.");

        var repository = _unitOfWork.Repository<Product>();

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

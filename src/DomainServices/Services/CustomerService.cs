using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;

namespace DomainServices;

public class CustomerService : BaseService, ICustomerService
{
    public CustomerService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory) { }

    public long CreateCustomer(Customer customer)
    {
        var unitOfWork = UnitOfWork.Repository<Customer>();
        VerifyCustomerAlreadyExists(customer);

        unitOfWork.Add(customer);
        UnitOfWork.SaveChanges();

        return customer.Id;
    }

    private void VerifyCustomerAlreadyExists(Customer customer)
    {
        var repository = RepositoryFactory.Repository<Customer>();

        if (repository.Any(x => x.Email.Equals(customer.Email)))
            throw new BadRequestException($"Customer already exists for email: {customer.Email}");

        if (repository.Any(x => x.Cpf.Equals(customer.Cpf)))
            throw new BadRequestException($"Customer already exists for CPF: {customer.Cpf}");
    }

    public IEnumerable<Customer> GetAll()
    {
        var repository = RepositoryFactory.Repository<Customer>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query); 
    }

    public Customer GetById(long id)
    {
        var repository = RepositoryFactory.Repository<Customer>();
        var customerFound = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(customerFound);
    }

    public IEnumerable<Customer> GetAllByFullName(string fullName)
    {
        var repository = RepositoryFactory.Repository<Customer>();
        var query = repository.MultipleResultQuery()
            .AndFilter(x => x.FullName.Contains(fullName));

        return repository.Search(query);
    }

    private Customer CustomerAlreadyExists(long id)
    {
        var customerFound = GetById(id);
        if (customerFound is null) throw new NotFoundException($"Client for Id: {id} not found.");

        return customerFound;
    }

    public void Update(long id, Customer customer)
    {
        var unitOfWork = UnitOfWork.Repository<Customer>();
        VerifyCustomerAlreadyExists(customer);
        var updatedCustomer = CustomerAlreadyExists(id);
        
        customer.Id = id;
        customer.CreatedAt = updatedCustomer.CreatedAt;

        unitOfWork.Update(customer);
        UnitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var unitOfWork = UnitOfWork.Repository<Customer>();
        CustomerAlreadyExists(id);

        unitOfWork.Remove(x => x.Id.Equals(id));
    }
}
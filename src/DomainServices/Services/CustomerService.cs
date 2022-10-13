using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;

namespace DomainServices;

public class CustomerService : BaseService, ICustomerService
{
    private readonly IRepository<Customer> _customerService;
    
    public CustomerService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory)
    {
        _customerService = RepositoryFactory.Repository<Customer>();
    }

    public long CreateCustomer(Customer customer)
    {
        var repository = UnitOfWork.Repository<Customer>();
        VerifyCustomerAlreadyExists(customer);
        
        repository.Add(customer);
        UnitOfWork.SaveChanges();

        return customer.Id;
    }

    private void VerifyCustomerAlreadyExists(Customer customer)
    {
        if (_customerService.Any(x => x.Email.Equals(customer.Email)))
            throw new BadRequestException($"Customer already exists for email: {customer.Email}");

        if (_customerService.Any(x => x.Cpf.Equals(customer.Cpf)))
            throw new BadRequestException($"Customer already exists for CPF: {customer.Cpf}");
    }

    public IEnumerable<Customer> GetAll()
    {
        var query = _customerService.MultipleResultQuery();

        return _customerService.Search(query); 
    }

    public Customer GetById(long id)
    {
        var customerFound = _customerService.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return _customerService.SingleOrDefault(customerFound);
    }

    public IEnumerable<Customer> GetAllByFullName(string fullName)
    {
        var query = _customerService.MultipleResultQuery()
            .AndFilter(x => x.FullName.Contains(fullName));

        return _customerService.Search(query);
    }

    private Customer CustomerAlreadyExists(long id)
    {
        var customerFound = GetById(id);
        if (customerFound is null) throw new NotFoundException($"Client for Id: {id} not found.");

        return customerFound;
    }

    public void Update(long id, Customer customer)
    {
        var repository = UnitOfWork.Repository<Customer>();
        VerifyCustomerAlreadyExists(customer);
        var updatedCustomer = CustomerAlreadyExists(id);
        
        customer.Id = id;
        customer.CreatedAt = updatedCustomer.CreatedAt;

        repository.Update(customer);
        UnitOfWork.SaveChanges();
    }

    public void Delete(long id)
    {
        var repository = UnitOfWork.Repository<Customer>();
        CustomerAlreadyExists(id);

        repository.Remove(x => x.Id.Equals(id));
    }
}
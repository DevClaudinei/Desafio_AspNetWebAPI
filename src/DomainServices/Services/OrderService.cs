using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;

namespace DomainServices.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryFactory _repositoryFactory;

    public OrderService(IUnitOfWork<ApplicationDbContext> unitOfWork, IRepositoryFactory<ApplicationDbContext> repositoryFactory)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
    }

    public long Create(Order order)
    {
        var repository = _unitOfWork.Repository<Order>();

        repository.Add(order);
        _unitOfWork.SaveChanges();

        return order.Id;
    }

    public IEnumerable<Order> GetAllOrder()
    {
        var repository = _repositoryFactory.Repository<Order>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public Order GetOrderById(long id)
    {
        var repository = _repositoryFactory.Repository<Order>();
        var customerFound = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(customerFound);
    }

    public IEnumerable<Order> GetQuantityOfQuotes(long portfolioId, long productId)
    {
        var repository = _repositoryFactory.Repository<Order>();
        var query = repository.SingleResultQuery()
            .AndFilter(x => x.ProductId.Equals(productId))
            .AndFilter(x => x.PortfolioId.Equals(portfolioId));

        return repository.Search(query);
    }

    public void Update(long id, Order order, long portfoliotId, long productId)
    {
        var repository = _unitOfWork.Repository<Order>();

        order.PortfolioId = portfoliotId;
        order.ProductId = productId;

        repository.Update(order);
        _unitOfWork.SaveChanges();

    }
}
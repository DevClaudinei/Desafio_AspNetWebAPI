using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public int GetQuantityOfQuotesBuy(long portfolioId, long productId)
    {
        var repository = _repositoryFactory.Repository<Order>();
        var quotesBuyed = repository.FromSql($"SELECT * FROM CustomerDB.Orders " +
            $"WHERE PortfolioId = {portfolioId} AND ProductId = {productId} AND Direction = 1")
            .Sum(x => x.Quotes);
        var quotesSelled = repository.FromSql($"SELECT * FROM CustomerDB.Orders " +
            $"WHERE PortfolioId = {portfolioId} AND ProductId = {productId} AND Direction = 2")
            .Sum(x => x.Quotes);
        var totalQuotes = quotesSelled - quotesBuyed;

        return totalQuotes;
    }

    public void Update(long id, Order order, long portfoliotId, long productId)
    {
        var repository = _unitOfWork.Repository<Order>();

        if (repository.Any(x => x.Id.Equals(order.Id)))
        {
            repository.Update(order);
            _unitOfWork.SaveChanges();
        }
    }
}
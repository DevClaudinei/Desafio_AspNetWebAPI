using DomainModels.Entities;
using DomainModels.Enum;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.Repository.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;

namespace DomainServices.Services;

public class OrderService : BaseService, IOrderService
{
    private readonly IRepository<Order> _orderService;

    public OrderService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory)
    {
        _orderService = RepositoryFactory.Repository<Order>();
    }

    public long Create(Order order)
    {
        var repository = UnitOfWork.Repository<Order>();

        repository.Add(order);
        UnitOfWork.SaveChanges();

        return order.Id;
    }

    public IEnumerable<Order> GetAll()
    {
        var query = _orderService.MultipleResultQuery();

        return _orderService.Search(query);
    }

    public Order GetById(long id)
    {
        var customerFound = _orderService.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return _orderService.SingleOrDefault(customerFound);
    }

    private IEnumerable<Order> GetAllWithParameters(long portfolioId, long productId)
    {
        var query = _orderService.MultipleResultQuery()
            .AndFilter(x => x.PortfolioId.Equals(portfolioId))
            .AndFilter(x => x.ProductId.Equals(productId));

        return _orderService.Search(query);
    }

    public int GetQuantityOfQuotes(long portfolioId, long productId)
    {
        var orders = GetAllWithParameters(portfolioId, productId);
        var quantity = 0;

        foreach (var order in orders)
        {
            if (order.Direction == OrderDirection.Buy)
                quantity += order.Quotes;
            if (order.Direction == OrderDirection.Sell)
                quantity -= order.Quotes;
        }

        return quantity;
    }

    public void Update(long id, Order order)
    {
        var repository = UnitOfWork.Repository<Order>();

        if (_orderService.Any(x => x.Id.Equals(order.Id)))
        {
            repository.Update(order);
            UnitOfWork.SaveChanges();
        }
    }
}
using DomainModels.Entities;
using DomainModels.Enum;
using DomainServices.Services.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;

namespace DomainServices.Services;

public class OrderService : BaseService, IOrderService
{
    public OrderService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        IRepositoryFactory<ApplicationDbContext> repositoryFactory
    ) : base(unitOfWork, repositoryFactory) { }

    public long Create(Order order)
    {
        var unitOfWork = UnitOfWork.Repository<Order>();

        unitOfWork.Add(order);
        UnitOfWork.SaveChanges();

        return order.Id;
    }

    public IEnumerable<Order> GetAll()
    {
        var repository = RepositoryFactory.Repository<Order>();
        var query = repository.MultipleResultQuery();

        return repository.Search(query);
    }

    public Order GetById(long id)
    {
        var repository = RepositoryFactory.Repository<Order>();
        var customerFound = repository.SingleResultQuery()
            .AndFilter(x => x.Id.Equals(id));

        return repository.SingleOrDefault(customerFound);
    }

    private IEnumerable<Order> GetAllWithParameters(long portfolioId, long productId)
    {
        var repository = RepositoryFactory.Repository<Order>();
        var query = repository.MultipleResultQuery()
            .AndFilter(x => x.PortfolioId.Equals(portfolioId))
            .AndFilter(x => x.ProductId.Equals(productId));

        return repository.Search(query);
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
}
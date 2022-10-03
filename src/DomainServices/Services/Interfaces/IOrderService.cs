using Application.Models.Enum;
using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IOrderService
{
    long Create(Order order);
    IEnumerable<Order> GetAllOrder();
    public Order GetOrderById(long id);
    public int GetQuantityOfQuotesBuy(long portfolioId, long productId);
    void Update(long id, Order order, long portfoliotId, long productId);
}
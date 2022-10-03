using Application.Models.Enum;
using DomainModels.Entities;
using System.Collections.Generic;

namespace DomainServices.Services.Interfaces;

public interface IOrderService
{
    long Create(Order order);
    IEnumerable<Order> GetAll();
    Order GetById(long id);
    int GetQuantityOfQuotes(long portfolioId, long productId);
    void Update(long id, Order order);
}
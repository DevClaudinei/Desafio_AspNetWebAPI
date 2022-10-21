using Application.Models.Order;
using DomainModels.Entities;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IOrderAppService
{
    long Create(Order investment);
    IEnumerable<OrderResult> GetAll();
    OrderResult GetOrderById(long id);
    int GetQuantityOfQuotes(long portfolioId, long productId);
    }
﻿using Application.Models.Order;
using DomainModels.Entities;
using System.Collections.Generic;

namespace AppServices.Services.Interfaces;

public interface IOrderAppService
{
    long Create(Order investment);
    IEnumerable<OrderResult> GetAllOrder();
    OrderResult GetOrderById(long id);
    IEnumerable<OrderResult> GetQuantityOfQuotes(long portfolioId, long productId);
    void Update(long id, OrderResult order, long portfoliotId, long productId);
}
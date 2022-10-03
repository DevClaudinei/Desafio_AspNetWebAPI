using Application.Models.Order;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AppServices.Services;

public class OrderAppService : IOrderAppService
{
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;
    
    public OrderAppService(
        IMapper mapper,
        IOrderService orderService
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    public long Create(Order investment)
    {
        return _orderService.Create(investment);
    }

    public Order Get(long id)
    {
        var order = _orderService.GetById(id);
        if (order is null) throw new NotFoundException($"Order for id: {id} not found.");

        return order;
    }

    public IEnumerable<OrderResult> GetAll()
    {
        var order = _orderService.GetAll();
        return _mapper.Map<IEnumerable<OrderResult>>(order);
    }

    public OrderResult GetOrderById(long id)
    {
        var order = _orderService.GetById(id);
        if (order is null) throw new NotFoundException($"Order for id: {id} not found.");

        return _mapper.Map<OrderResult>(order);
    }

    public int GetQuantityOfQuotes(long portfolioId, long productId)
    {
        return _orderService.GetQuantityOfQuotes(portfolioId, productId);
    }

    public void Update(long id, OrderResult order)
    {
        var orderToUpdate = _mapper.Map<Order>(order);

        _orderService.Update(id, orderToUpdate);
    }
}
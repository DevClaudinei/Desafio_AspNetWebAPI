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

    public IEnumerable<OrderResult> GetAllOrder()
    {
        var order = _orderService.GetAllOrder();
        return _mapper.Map<IEnumerable<OrderResult>>(order);
    }

    public OrderResult GetOrderById(long id)
    {
        var order = _orderService.GetOrderById(id);
        if (order is null) throw new NotFoundException($"Order for id: {id} not found.");

        return _mapper.Map<OrderResult>(order);
    }

    public IEnumerable<OrderResult> GetQuantityOfQuotes(long portfolioId, long productId)
    {
        var order = _orderService.GetQuantityOfQuotes(portfolioId, productId);
        if (order is null) throw new NotFoundException($"Não existe order contendo {portfolioId} e {productId}");

        return _mapper.Map<IEnumerable<OrderResult>>(order);
    }

    public void Update(long id, OrderResult order, long portfoliotId, long productId)
    {
        var orderToUpdate = _mapper.Map<Order>(order);

        _orderService.Update(id, orderToUpdate, portfoliotId, productId);
    }
}
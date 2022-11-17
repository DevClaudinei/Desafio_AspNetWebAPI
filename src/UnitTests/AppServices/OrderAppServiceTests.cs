using AppServices.Profiles;
using AppServices.Services;
using AutoMapper;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using UnitTests.EntitiesFake.Orders;
using UnitTests.EntitiesFake.Portfolios;

namespace UnitTests.AppServices;

public class OrderAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly OrderAppService _orderAppService;
    private readonly Mock<IOrderService> _orderService;

    public OrderAppServiceTests()
    {
        var config = new MapperConfiguration(opt =>
        {
            opt.AddProfile(new OrderProfile());
        });
        _mapper = config.CreateMapper();
        _orderService = new Mock<IOrderService>();
        _orderAppService = new OrderAppService(_mapper, _orderService.Object);
    }

    [Fact]
    public void Should_Create_Order_Sucessfully()
    {
        // Arrange
        var orderFake = OrderFake.OrderFaker();

        _orderService.Setup(x => x.Create(orderFake)).Returns(orderFake.Id);

        // Act
        var orderId = _orderAppService.Create(orderFake);

        // Assert
        orderId.Should().Be(orderFake.Id);
        _orderService.Verify(x => x.Create(orderFake), Times.Once());
    }

    [Fact]
    public void Should_Return_Orders_When_Executing_GetAll()
    {
        // Arrange
        var orderFake = OrderFake.OrderFakers(2);

        _orderService.Setup(x => x.GetAll()).Returns(orderFake);

        //Act
        var ordersFound = _orderAppService.GetAll();

        // Assert
        ordersFound.Should().HaveCount(2);
        _orderService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_Return_Empty_When_Executing_GetAll()
    {
        // Arrange
        var orderResultFake = OrderResponseModel.OrderFakers(2);

        _orderService.Setup(x => x.GetAll());

        //Act
        var ordersFound = _orderAppService.GetAll();

        // Assert
        ordersFound.Should().BeEmpty();
        _orderService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetOrderById()
    {
        // Arrange
        var orderFake = OrderFake.OrderFaker();

        _orderService.Setup(x => x.GetById(orderFake.Id)).Returns(orderFake);

        // Act
        var orderFound = _orderAppService.GetOrderById(orderFake.Id);

        // Assert
        orderFound.Id.Should().Be(orderFake.Id);
        _orderService.Verify(x => x.GetById(orderFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetOrderById()
    {
        // Arrange
        var orderFake = OrderFake.OrderFaker();

        _orderService.Setup(x => x.GetById(orderFake.Id));

        // Act
        Action act = () => _orderAppService.GetOrderById(orderFake.Id);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Order for id: {orderFake.Id} not found.");
        _orderService.Verify(x => x.GetById(orderFake.Id), Times.Once());
    }

    [Fact]
    public void Should_GetQuantityOfQuotes_In_Order_Sucessfully()
    {
        // Arrange
        var orderFake = OrderFake.OrderFaker();

        _orderService.Setup(x => x.GetQuantityOfQuotes(orderFake.PortfolioId, orderFake.ProductId))
            .Returns(orderFake.Quotes);

        // Act
        var quotes = _orderAppService.GetQuantityOfQuotes(orderFake.PortfolioId, orderFake.ProductId);

        // Assert
        quotes.Should().Be(orderFake.Quotes);
        _orderService.Verify(x => x.GetQuantityOfQuotes(orderFake.PortfolioId, orderFake.ProductId), Times.Once());
    }
}
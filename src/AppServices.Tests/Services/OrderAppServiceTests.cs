using AppServices.Profiles;
using AppServices.Services;
using AppServices.Tests.ModelsFake.Order;
using AutoMapper;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using DomainServices.Tests.EntitiesFake;
using FluentAssertions;
using Moq;

namespace AppServices.Tests.Services;

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
    public void Should_Create_Sucessfully()
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
    public void Should_GetAll_When_OrdersExists()
    {
        // Arrange
        var orderResultFake = OrderResponseModel.OrderFakers(2);
        var orderFake = OrderFake.OrderFakers(2);

        _orderService.Setup(x => x.GetAll()).Returns(orderFake);

        //Act
        var ordersFound = _orderAppService.GetAll();

        // Assert
        ordersFound.Should().HaveCountGreaterThan(0);
        _orderService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_OrdersDoesNotExists()
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
    public void Should_GetOrderById_When_OrderExists()
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
    public void Should_GetOrderById_When_OrderDoesNotExists()
    {
        // Arrange
        var orderFake = OrderFake.OrderFaker();

        _orderService.Setup(x => x.GetById(orderFake.Id));

        // Act
        Action act = () => _orderAppService.GetOrderById(orderFake.Id);

        // Assert
        act.Should().Throw<NotFoundException>($"Order for id: {orderFake.Id} not found.");
        _orderService.Verify(x => x.GetById(orderFake.Id), Times.Once());
    }

    [Fact]
    public void Should_GetQuantityOfQuotes_Sucessfully()
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
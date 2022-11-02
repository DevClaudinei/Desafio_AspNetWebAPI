﻿using DomainModels.Entities;
using DomainServices.Services;
using DomainServices.Tests.EntitiesFake;
using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Moq;
using System.Linq.Expressions;

namespace DomainServices.Tests.Services;

public class OrderServiceTests
{
    private readonly OrderService _orderService;
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;

    public OrderServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();
        _mockRepositoryFactory = new Mock<IRepositoryFactory<ApplicationDbContext>>();
        _orderService = new OrderService(_mockUnitOfWork.Object, _mockRepositoryFactory.Object);
    }

    [Fact]
    public void Should_Create_Sucessfully()
    {
        // Arrange
        var orderFake = OrderFake.OrderFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Order>()
            .Add(It.IsAny<Order>()))
            .Returns(It.IsAny<Order>());
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .Any(It.IsAny<Expression<Func<Order, bool>>>()));

        // Act
        var orderId = _orderService.Create(orderFake);

        // Assert
        orderId.Should().Be(orderFake.Id);

        _mockUnitOfWork.Verify(x => x.Repository<Order>().Add(orderFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_OrdersExists()
    {
        // Arrange
        var orderFakers = OrderFake.OrderFakers(5);

        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<Order>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .Search(It.IsAny<IMultipleResultQuery<Order>>()))
            .Returns((IList<Order>)orderFakers);

        // Act
        var ordersFound = _orderService.GetAll();

        // Assert
        ordersFound.Should().HaveCount(5);

        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .Search(It.IsAny<IMultipleResultQuery<Order>>()), Times.Once());
    }

    [Fact]
    public void Should_NotGetAll_When_OrdersDoesNotExists()
    {
        // Arrange
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<Order>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .Search(It.IsAny<IMultipleResultQuery<Order>>()))
            .Returns(new List<Order>());

        // Act
        var ordersFound = _orderService.GetAll();

        // Assert
        ordersFound.Should().BeEmpty();

        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .Search(It.IsAny<IMultipleResultQuery<Order>>()), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_OrderExists()
    {
        // Arrange
        var orderFake = OrderFake.OrderFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()))
            .Returns(It.IsAny<IQuery<Order>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .SingleOrDefault(It.IsAny<IQuery<Order>>()))
            .Returns(orderFake);

        // Act
        var orderFound = _orderService.GetById(orderFake.Id);

        // Assert
        orderFound.Id.Should().Be(orderFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .SingleOrDefault(It.IsAny<IQuery<Order>>()), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_OrderDoesNotExists()
    {
        // Arrange
        var orderFake = OrderFake.OrderFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()))
            .Returns(It.IsAny<IQuery<Order>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .SingleOrDefault(It.IsAny<IQuery<Order>>()));

        // Act
        var orderFound = _orderService.GetById(orderFake.Id);

        // Assert
        orderFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .SingleOrDefault(It.IsAny<IQuery<Order>>()), Times.Once());
    }

    [Fact]
    public void Should_GetQuantityOfQuotes_Sucessfully()
    {
        // Arrange
        var orderFakes = OrderFake.OrderFakers(5);
        var orderFake = orderFakes.ElementAt(1);
        
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()))
            .Returns(It.IsAny<IMultipleResultQuery<Order>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .Search(It.IsAny<IQuery<Order>>()))
            .Returns((IList<Order>)orderFakes);
        
        // Act
        var quantityOfQuotes = _orderService
            .GetQuantityOfQuotes(orderFake.PortfolioId, orderFake.ProductId);

        // Assert
        quantityOfQuotes.Should().Be(1);

        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .Search(It.IsAny<IQuery<Order>>()), Times.Once());
    }
}
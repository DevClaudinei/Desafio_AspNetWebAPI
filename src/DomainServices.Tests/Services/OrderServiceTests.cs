using DomainModels.Entities;
using DomainServices.Services;
using DomainServices.Services.Interfaces;
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
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();
        _mockRepositoryFactory = new Mock<IRepositoryFactory<ApplicationDbContext>>();
        _orderService = new OrderService(_mockUnitOfWork.Object, _mockRepositoryFactory.Object);
    }

    [Fact]
    public void Should_CreateOrder_Sucessfully()
    {
        var orderFake = OrderFake.OrderFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Order>()
            .Add(It.IsAny<Order>()))
            .Returns(orderFake);
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .Any(It.IsAny<Expression<Func<Order, bool>>>()));

        var orderId = _orderService.Create(orderFake);
        orderId.Should().Be(orderFake.Id);

        _mockUnitOfWork.Verify(x => x.Repository<Order>(), Times.Exactly(1));
        _mockUnitOfWork.Verify(x => x.Repository<Order>().Add(orderFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_ReturnAllOrders_Sucessfully()
    {
        var orderFakers = OrderFake.OrderFakers(5);
        var query = Mock.Of<IMultipleResultQuery<Order>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .Search(query)).Returns((IList<Order>)orderFakers);

        var ordersFound = _orderService.GetAll();
        ordersFound.Should().HaveCount(5);

        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .MultipleResultQuery(), Times.Once());
    }

    [Fact]
    public void Should_NoReturns_When_NoCustomerBankInfosRegistered()
    {
        var query = Mock.Of<IMultipleResultQuery<Order>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .Search(query)).Returns(new List<Order>());

        var ordersFound = _orderService.GetAll();
        ordersFound.Should().BeEmpty();
    }

    [Fact]
    public void Should_ReturnCustomerBankInfo_When_AccountExist()
    {
        var orderFake = OrderFake.OrderFaker();
        var query = Mock.Of<IQuery<Order>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .SingleOrDefault(query)).Returns(orderFake);

        var orderFound = _orderService.GetById(orderFake.Id);
        orderFound.Id.Should().Be(orderFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_ReturnCustomerBankInfo_When_AccountDoesNotExist()
    {
        var orderFake = OrderFake.OrderFaker();
        var query = Mock.Of<IQuery<Order>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .SingleOrDefault(query));

        var orderFound = _orderService.GetById(orderFake.Id);
        orderFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_ReturnGetQuantityOfQuotes_When_RealizeUnivestimentAndProductExistsInThePortfolio()
    {
        var orderFakes = OrderFake.OrderFakers(5);
        var query = Mock.Of<IQuery<Order>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Order>()
            .Search(query)).Returns((IList<Order>)orderFakes);

        var orderFakesFound = _orderService
            .GetQuantityOfQuotes(orderFakes.ElementAt(1).PortfolioId, orderFakes.ElementAt(1).ProductId);
        orderFakesFound.Should().BeGreaterThanOrEqualTo(0);
        _mockRepositoryFactory.Verify(x => x.Repository<Order>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once());
    }
}

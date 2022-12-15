using AppServices.Profiles;
using AppServices.Services;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using UnitTests.EntitiesFake.Portfolios;
using UnitTests.EntitiesFake.Products;

namespace UnitTests.AppServices;

public class PortfolioAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly PortfolioAppService _portfolioAppService;
    private readonly Mock<IOrderAppService> _mockOrderAppService;
    private readonly Mock<IPortfolioService> _mockPortfolioService;
    private readonly Mock<IProductAppService> _mockProductAppService;
    private readonly Mock<IPortfolioProductAppService> _mockPortfolioProductAppService;
    private readonly Mock<ICustomerBankInfoAppService> _mockCustomerBankInfoAppService;

    public PortfolioAppServiceTests()
    {
        var config = new MapperConfiguration(opt =>
        {
            opt.AddProfile(
                new PortfolioProfile()
            );
            opt.AddProfile(
                new ProductProfile()
            );
            opt.AddProfile(
                new OrderProfile()
            );
        });
        _mapper = config.CreateMapper();
        _mockOrderAppService = new Mock<IOrderAppService>();
        _mockPortfolioService = new Mock<IPortfolioService>();
        _mockProductAppService = new Mock<IProductAppService>();
        _mockPortfolioProductAppService = new Mock<IPortfolioProductAppService>();
        _mockCustomerBankInfoAppService = new Mock<ICustomerBankInfoAppService>();
        _portfolioAppService = new PortfolioAppService(
            _mapper, _mockOrderAppService.Object, _mockPortfolioService.Object,
            _mockProductAppService.Object, _mockPortfolioProductAppService.Object,
            _mockCustomerBankInfoAppService.Object
        );
    }

    [Fact]
    public void Should_Pass_When_Executing_Create_Portfolio()
    {
        // Arrange
        var portfolioModelFake = CreatePortfolioModel.PortfolioFake();
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockPortfolioService.Setup(x => x.Create(It.IsAny<Portfolio>())).Returns(portfolioFake.Id);

        _mockCustomerBankInfoAppService.Setup(x => x.GetCustomerBankInfoId(portfolioFake.CustomerId)).Returns(portfolioFake.CustomerId);

        // Act
        var portfolioId = _portfolioAppService.Create(portfolioModelFake);

        // Assert
        portfolioId.Should().Be(portfolioFake.Id);

        _mockPortfolioService.Verify(x => x.Create(It.IsAny<Portfolio>()), Times.Once());

        _mockCustomerBankInfoAppService.Verify(x => x.GetCustomerBankInfoId(portfolioFake.CustomerId), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_Create_Portfolio()
    {
        // Arrange
        var portfolioModelFake = CreatePortfolioModel.PortfolioFake();
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockCustomerBankInfoAppService.Setup(x => x.GetCustomerBankInfoId(portfolioFake.CustomerId)).Returns(0);

        // Act
        Action act = () => _portfolioAppService.Create(portfolioModelFake);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Customer for Id: {portfolioFake.CustomerId} not found");

        _mockCustomerBankInfoAppService.Verify(x => x.GetCustomerBankInfoId(portfolioFake.CustomerId), Times.Once());
    }

    [Fact]
    public void Should_Return_Portfolios_When_Executing_GetAll()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFakers(1);

        _mockPortfolioService.Setup(x => x.GetAll()).Returns(portfolioFake);

        // Act
        var portfoliosFakeFound = _portfolioAppService.GetAll();

        // Assert
        portfoliosFakeFound.Should().HaveCount(1);

        _mockPortfolioService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_Return_Empty_When_Executing_GetAll()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFakers(1);

        _mockPortfolioService.Setup(x => x.GetAll());

        // Act
        var portfoliosFakeFound = _portfolioAppService.GetAll();

        // Assert
        portfoliosFakeFound.Should().BeEmpty();

        _mockPortfolioService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetPortfolioById()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockPortfolioService.Setup(x => x.GetById(portfolioFake.Id)).Returns(portfolioFake);

        // Act
        var portfolioFound = _portfolioAppService.GetPortfolioById(portfolioFake.Id);

        // Assert
        portfolioFound.Should().NotBeNull();

        _mockPortfolioService.Verify(x => x.GetById(portfolioFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetPortfolioById()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockPortfolioService.Setup(x => x.GetById(portfolioFake.Id));

        // Act
        Action act = () => _portfolioAppService.GetPortfolioById(portfolioFake.Id);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Portfolio for Id: {portfolioFake.Id} not found.");

        _mockPortfolioService.Verify(x => x.GetById(portfolioFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetTotalBalance()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockPortfolioService.Setup(x => x.GetById(portfolioFake.Id)).Returns(portfolioFake);

        // Act
        var totalBalance = _portfolioAppService.GetTotalBalance(portfolioFake.Id);

        // Assert
        totalBalance.Should().Be(portfolioFake.TotalBalance);

        _mockPortfolioService.Verify(x => x.GetById(portfolioFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetTotalBalance()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockPortfolioService.Setup(x => x.GetById(portfolioFake.Id));

        // Act
        Action act = () => _portfolioAppService.GetTotalBalance(portfolioFake.Id);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Portfolio for Id: {portfolioFake.Id} not found.");

        _mockPortfolioService.Verify(x => x.GetById(portfolioFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_Delete_A_Portfolio()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var amount = 0;

        _mockPortfolioService.Setup(x => x.GetById(portfolioFake.Id)).Returns(portfolioFake);

        _mockPortfolioService.Setup(x => x.GetTotalBalance(portfolioFake.Id)).Returns(portfolioFake.TotalBalance);

        _mockCustomerBankInfoAppService.Setup(x => x.Withdraw(portfolioFake.CustomerId, amount));

        // Act
        _portfolioAppService.Delete(portfolioFake.Id);

        // Assert
        _mockPortfolioService.Verify(x => x.GetById(portfolioFake.Id), Times.Once());

        _mockPortfolioService.Verify(x => x.GetTotalBalance(portfolioFake.Id), Times.Once());

        _mockCustomerBankInfoAppService.Verify(x => x.Withdraw(portfolioFake.CustomerId, amount), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_Delete_A_Portfolio()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockPortfolioService.Setup(x => x.GetById(portfolioFake.Id));

        // Act
        Action act = () => _portfolioAppService.Delete(portfolioFake.Id);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Portfolio for Id: {portfolioFake.Id} not found.");

        _mockPortfolioService.Verify(x => x.GetById(portfolioFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_Investment_In_A_Portfolio()
    {
        // Arrange
        var requestFake = CreateInvestmentModel.InvestmentFake();
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var productFake = ProductFake.ProductFaker();
        var orderFake = OrderFake.OrderFaker();

        _mockPortfolioService.Setup(x => x.GetById(requestFake.PortfolioId)).Returns(portfolioFake);

        _mockProductAppService.Setup(x => x.Get(requestFake.ProductId)).Returns(productFake);

        _mockOrderAppService.Setup(x => x.Create(It.IsAny<Order>())).Returns(orderFake.Id);

        // Act
        var orderId = _portfolioAppService.Invest(requestFake);

        // Assert
        orderId.Should().Be(orderFake.Id);

        _mockPortfolioService.Verify(x => x.GetById(requestFake.PortfolioId), Times.Once());

        _mockProductAppService.Verify(x => x.Get(requestFake.ProductId), Times.Once());

        _mockOrderAppService.Verify(x => x.Create(It.IsAny<Order>()), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_Investment_In_A_Portfolio()
    {
        // Arrange
        var requestFake = CreateInvestmentModel.InvestmentFake();
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var productFake = ProductFake.ProductFaker();
        var orderFake = OrderFake.OrderFaker();

        _mockPortfolioService.Setup(x => x.GetById(requestFake.PortfolioId));

        // Act
        Action act = () => _portfolioAppService.Invest(requestFake);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Portfolio for Id: {requestFake.PortfolioId} not found.");

        _mockPortfolioService.Verify(x => x.GetById(requestFake.PortfolioId), Times.Once);
    }

    [Fact]
    public void Should_Pass_When_Executing_Uninvest_In_A_Portfolio()
    {
        // Arrange
        var requestFake = CreateUninvestmentModel.UninvestmentFake();
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var productFake = ProductFake.ProductFaker();
        var orderFake = OrderFake.OrderFaker();

        _mockPortfolioService.Setup(x => x.GetById(requestFake.PortfolioId)).Returns(portfolioFake);

        _mockProductAppService.Setup(x => x.Get(requestFake.ProductId)).Returns(productFake);

        _mockOrderAppService.Setup(x => x.Create(It.IsAny<Order>())).Returns(orderFake.Id);

        // Act
        var orderId = _portfolioAppService.Uninvest(requestFake);

        // Assert
        orderId.Should().Be(orderFake.Id);

        _mockPortfolioService.Verify(x => x.GetById(requestFake.PortfolioId), Times.Once());

        _mockProductAppService.Verify(x => x.Get(requestFake.ProductId), Times.Once());

        _mockOrderAppService.Verify(x => x.Create(It.IsAny<Order>()), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_Uninvest_In_A_Portfolio()
    {
        // Arrange
        var requestFake = CreateUninvestmentModel.UninvestmentFake();
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var productFake = ProductFake.ProductFaker();
        var orderFake = OrderFake.OrderFaker();

        _mockPortfolioService.Setup(x => x.GetById(requestFake.PortfolioId));

        // Act
        Action act = () => _portfolioAppService.Uninvest(requestFake);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Portfolio for Id: {requestFake.PortfolioId} not found.");

        _mockPortfolioService.Verify(x => x.GetById(requestFake.PortfolioId), Times.Once);
    }

    [Fact]
    public void Should_Return_Portflios_When_Executing_GetAllByCustomerId()
    {
        // Arrange
        var portfoliosFake = PortfolioFake.PortfolioFakers(2);
        var portfoliofake = portfoliosFake.First();

        _mockPortfolioService.Setup(x => x.GetAllByCustomerId(portfoliofake.Id)).Returns(portfoliosFake);

        // Act
        var portfolioFound = _portfolioAppService.GetAllByCustomerId(portfoliofake.Id);

        // Assert
        portfolioFound.Should().HaveCountGreaterThan(1);

        _mockPortfolioService.Verify(x => x.GetAllByCustomerId(portfoliofake.Id), Times.Once());
    }

    [Fact]
    public void Should_Return_Empty_When_Executing_GetAllByCustomerId()
    {
        // Arrange
        var portfoliosFake = PortfolioFake.PortfolioFakers(2);
        var portfoliofake = portfoliosFake.First();

        _mockPortfolioService.Setup(x => x.GetAllByCustomerId(portfoliofake.Id));

        // Act
        var portfolioFound = _portfolioAppService.GetAllByCustomerId(portfoliofake.Id);

        // Assert
        portfolioFound.Should().BeEmpty();

        _mockPortfolioService.Verify(x => x.GetAllByCustomerId(portfoliofake.Id), Times.Once());
    }
}
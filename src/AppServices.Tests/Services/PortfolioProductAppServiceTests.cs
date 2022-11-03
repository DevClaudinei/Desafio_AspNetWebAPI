using AppServices.Profiles;
using AppServices.Services;
using AutoMapper;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using DomainServices.Tests.EntitiesFake;
using FluentAssertions;
using Moq;

namespace AppServices.Tests.Services;

public class PortfolioProductAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IPortfolioProductService> _mockPortfolioProductService;
    private readonly PortfolioProductAppService _portfolioProductAppService;

    public PortfolioProductAppServiceTests()
    {
        var config = new MapperConfiguration(opt =>
        {
            opt.AddProfile(
                new PortfolioProductProfile()
            );
        });
        _mapper = config.CreateMapper();
        _mockPortfolioProductService = new Mock<IPortfolioProductService>();
        _portfolioProductAppService = new PortfolioProductAppService(
            _mapper, _mockPortfolioProductService.Object
        );
    }

    [Fact]
    public void Should_GetAll_When_PortfolioProductsExists()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFakers(2);

        _mockPortfolioProductService.Setup(x => x.GetAll()).Returns(portfolioProductFake);

        // Act
        var portfolioProductFound = _portfolioProductAppService.GetAll();

        // Assert
        portfolioProductFound.Should().HaveCount(2);
        _mockPortfolioProductService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_PortfolioProductsDoesNotExists()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFakers(2);

        _mockPortfolioProductService.Setup(x => x.GetAll());

        // Act
        var portfolioProductFound = _portfolioProductAppService.GetAll();

        // Assert
        portfolioProductFound.Should().BeEmpty();
        _mockPortfolioProductService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_PortfolioProductExists()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();
        
        _mockPortfolioProductService.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<long>()))
            .Returns(portfolioProductFake);

        // Act
        var portfolioProductFound = _portfolioProductAppService.GetById(portfolioProductFake.PortfolioId, portfolioProductFake.ProductId);

        // Assert
        portfolioProductFound.Should().NotBeNull();
        _mockPortfolioProductService.Verify(x => x.GetById(It.IsAny<long>(), It.IsAny<long>()), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_PortfolioProductDoesNotExists()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockPortfolioProductService.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<long>()));

        // Act
        Action act = () => _portfolioProductAppService.GetById(portfolioProductFake.PortfolioId, portfolioProductFake.ProductId);

        // Assert
        act.Should().Throw<NotFoundException>($"PortfolioProduct with Portfolioid: {portfolioProductFake.PortfolioId} and ProductId: {portfolioProductFake.ProductId} not found.");
        _mockPortfolioProductService.Verify(x => x.GetById(It.IsAny<long>(), It.IsAny<long>()), Times.Once());
    }

    [Fact]
    public void Should_AddProduct_Sucessfully()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockPortfolioProductService.Setup(x => x.AddProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product));

        // Act
        _portfolioProductAppService.AddProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockPortfolioProductService.Verify(x => x.AddProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product), Times.Once());
    }

    [Fact]
    public void Should_RemoveProduct_Sucessfully()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockPortfolioProductService.Setup(x => x.RemoveProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product));

        // Act
        _portfolioProductAppService.RemoveProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockPortfolioProductService.Verify(x => x.RemoveProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product), Times.Once());
    }
}
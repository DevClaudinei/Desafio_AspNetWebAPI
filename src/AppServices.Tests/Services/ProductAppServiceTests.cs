using AppServices.Profiles;
using AppServices.Services;
using AppServices.Tests.ModelsFake.Product;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using DomainServices.Tests.EntitiesFake;
using FluentAssertions;
using Moq;

namespace AppServices.Tests.Services;

public class ProductAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductAppService _productAppService;

    public ProductAppServiceTests()
    {
        var config = new MapperConfiguration(opt =>
        {
            opt.AddProfile(
                new ProductProfile()
            );
        });
        _mapper = config.CreateMapper();
        _mockProductService = new Mock<IProductService>();
        _productAppService = new ProductAppService(
            _mockProductService.Object, _mapper
        );
    }

    [Fact]
    public void Should_Create_Sucessfully()
    {
        // Arrange
        var productModel = CreateProductModel.ProductFake();
        
        _mockProductService.Setup(x => x.Create(It.IsAny<Product>())).Returns(productModel.Id);

        // Act
        var productId = _productAppService.Create(productModel);

        // Assert
        productId.Should().Be(productModel.Id);
        _mockProductService.Verify(x => x.Create(It.IsAny<Product>()), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_ProductsExist()
    {
        // Arrange
        var productsFakes = ProductFake.ProductFakers(2);

        _mockProductService.Setup(x => x.GetAll()).Returns((IList<Product>)productsFakes);

        // Act
        var productsFound = _productAppService.GetAll();

        // Assert
        productsFound.Should().HaveCount(2);
        _mockProductService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_NotGetAll_When_ProductsDoesNotExist()
    {
        // Arrange
        var productsFakes = ProductFake.ProductFakers(2);

        _mockProductService.Setup(x => x.GetAll());

        // Act
        var productsFound = _productAppService.GetAll();

        // Assert
        productsFound.Should().BeEmpty();
        _mockProductService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_GetBySymbol_When_ProductExists() 
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();
        
        _mockProductService.Setup(x => x.GetBySymbol(It.IsAny<string>()))
            .Returns(productFake);

        // Act
        var productFound = _productAppService.GetBySymbol(productFake.Symbol);

        // Assert
        productFound.Symbol.Should().Be(productFake.Symbol);
        _mockProductService.Verify(x => x.GetBySymbol(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public void Should_NotGetBySymbol_When_ProductDoesNotExists()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockProductService.Setup(x => x.GetBySymbol(It.IsAny<string>()));

        // Act
        Action act = () => _productAppService.GetBySymbol(productFake.Symbol);

        // Assert
        act.Should().Throw< NotFoundException>($"Product for the symbol: {productFake.Symbol} was not found.");
        _mockProductService.Verify(x => x.GetBySymbol(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_ProductExists()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockProductService.Setup(x => x.GetById(It.IsAny<long>())).Returns(productFake);

        // Act
        var productFound = _productAppService.GetById(productFake.Id);

        // Assert
        productFound.Id.Should().Be(productFake.Id);
        _mockProductService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
    }

    [Fact]
    public void Should_NotGetById_When_ProductDoesNotExists()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockProductService.Setup(x => x.GetById(It.IsAny<long>()));

        // Act
        Action act = () => _productAppService.GetById(productFake.Id);

        // Assert
        act.Should().Throw<NotFoundException>($"Product for the Id: {productFake.Id} was not found.");
        _mockProductService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
    }

    [Fact]
    public void Should_Get_When_ProductExists()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockProductService.Setup(x => x.GetById(It.IsAny<long>()))
            .Returns(productFake);

        // Act
        var productFound = _productAppService.Get(productFake.Id);

        // Assert
        productFound.Id.Should().Be(productFake.Id);
        _mockProductService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
    }

    [Fact]
    public void Should_Get_When_ProductDoesNotExists()
    {
        // Arrange
        var productFake = UpdateProductModel.ProductFake();

        _mockProductService.Setup(x => x.GetById(It.IsAny<long>()));

        // Act
        Action act = () => _productAppService.Get(productFake.Id);

        // Assert
        act.Should().Throw<NotFoundException>($"Product for the Id: {productFake.Id} was not found.");
        _mockProductService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
    }

    [Fact]
    public void Should_Update_Sucessfully()
    {
        // Arrange
        var productFake = UpdateProductModel.ProductFake();
        
        _mockProductService.Setup(x => x.Update(It.IsAny<Product>()));

        // Act
        _productAppService.Update(productFake.Id, productFake);

        // Assert
        _mockProductService.Verify(x => x.Update(It.IsAny<Product>()), Times.Once());
    }

    [Fact]
    public void Should_Delete_Sucessfully()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockProductService.Setup(x => x.Delete(It.IsAny<long>()));

        // Act
        _productAppService.Delete(productFake.Id);

        // Assert
        _mockProductService.Verify(x => x.Delete(It.IsAny<long>()), Times.Once());
    }
}
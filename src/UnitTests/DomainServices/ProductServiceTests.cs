using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services;
using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnitTests.EntitiesFake.Products;

namespace UnitTests.DomainServices;

public class ProductServiceTests
{
    private readonly ProductService _productService;
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;

    public ProductServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();
        _mockRepositoryFactory = new Mock<IRepositoryFactory<ApplicationDbContext>>();
        _productService = new ProductService(
            _mockUnitOfWork.Object, _mockRepositoryFactory.Object
        );
    }

    [Fact]
    public void Should_Pass_When_Executing_Create_Product()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()));

        _mockUnitOfWork.Setup(x => x.Repository<Product>().Add(It.IsAny<Product>()));

        // Act
        var productId = _productService.Create(productFake);

        // Assert
        productId.Should().Be(productFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<Product>().Add(productFake), Times.Once());

        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_Create_Product()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>())).Returns(true);

        _mockUnitOfWork.Setup(x => x.Repository<Product>().Add(It.IsAny<Product>()));

        // Act
        Action act = () => _productService.Create(productFake);

        // Assert
        act.Should().ThrowExactly<BadRequestException>($"Product: {productFake.Symbol} are already registered");

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_Porducts_When_Executing_GetAll()
    {
        // Arrange
        var productFakes = ProductFake.ProductFakers(5);

        _mockRepositoryFactory.Setup(x => x.Repository<Product>().MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<Product>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Search(It.IsAny<IMultipleResultQuery<Product>>())).Returns((IList<Product>)productFakes);

        // Act
        var productsFound = _productService.GetAll();

        // Assert
        productsFound.Should().HaveCount(5);

        _mockRepositoryFactory.Verify(x => x.Repository<Product>().MultipleResultQuery(), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Search(It.IsAny<IMultipleResultQuery<Product>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_Empty_When_Executing_GetAll()
    {
        // Arrange
        _mockRepositoryFactory.Setup(x => x.Repository<Product>().MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<Product>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Search(It.IsAny<IMultipleResultQuery<Product>>())).Returns(new List<Product>());

        // Act
        var productsFound = _productService.GetAll();

        // Assert
        productsFound.Should().BeEmpty();

        _mockRepositoryFactory.Verify(x => x.Repository<Product>().MultipleResultQuery(), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Search(It.IsAny<IMultipleResultQuery<Product>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_Product_When_Executing_GetBySymbol()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(It.IsAny<IMultipleResultQuery<Product>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleOrDefault(It.IsAny<IMultipleResultQuery<Product>>())).Returns(productFake);

        // Act
        var productFound = _productService.GetBySymbol(productFake.Symbol);

        // Assert
        productFound.Symbol.Should().Be(productFake.Symbol);

        _mockRepositoryFactory.Verify(x => x.Repository<Product>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleOrDefault(It.IsAny<IMultipleResultQuery<Product>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_Null_When_Executing_GetBySymbol()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(It.IsAny<IMultipleResultQuery<Product>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleOrDefault(It.IsAny<IMultipleResultQuery<Product>>()));

        // Act
        var productFound = _productService.GetBySymbol(productFake.Symbol);

        // Assert
        productFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<Product>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleOrDefault(It.IsAny<IMultipleResultQuery<Product>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_Product_When_Executing_GetById()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(It.IsAny<IMultipleResultQuery<Product>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleOrDefault(It.IsAny<IMultipleResultQuery<Product>>())).Returns(productFake);

        // Act
        var productFound = _productService.GetById(productFake.Id);

        // Assert
        productFound.Id.Should().Be(productFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<Product>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleOrDefault(It.IsAny<IMultipleResultQuery<Product>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_Null_When_Executing_GetById()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(It.IsAny<IMultipleResultQuery<Product>>());

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleOrDefault(It.IsAny<IMultipleResultQuery<Product>>()));

        // Act
        var productFound = _productService.GetById(productFake.Id);

        // Assert
        productFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<Product>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleOrDefault(It.IsAny<IMultipleResultQuery<Product>>()), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_Update_Product()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Product>().Update(It.IsAny<Product>()));

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>())).Returns(true);

        // Act
        _productService.Update(productFake);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<Product>().Update(productFake), Times.Once());

        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_Update_Product()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>())).Returns(false);

        // Act
        Action act = () => _productService.Update(productFake);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Product not found for id: {productFake.Id}.");

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_Delete_A_Product()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Product>().Remove(It.IsAny<Product>()));

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>())).Returns(true);

        // Act
        _productService.Delete(productFake.Id);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<Product>()
            .Remove(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_Delete_A_Product()
    {
        // Arrange
        var productFake = ProductFake.ProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>())).Returns(false);

        // Act
        Action act = () => _productService.Delete(productFake.Id);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Product not found for id: {productFake.Id}.");

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
    }
}
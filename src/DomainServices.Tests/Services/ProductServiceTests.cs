using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services;
using DomainServices.Tests.EntitiesFake;
using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Moq;
using System.Linq.Expressions;

namespace DomainServices.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();
        _mockRepositoryFactory = new Mock<IRepositoryFactory<ApplicationDbContext>>();
        _productService = new ProductService(
            _mockUnitOfWork.Object, _mockRepositoryFactory.Object
        );
    }

    [Fact]
    public void Should_Create_When_ProductDoesNotExists()
    {
        var productFake = ProductFake.ProductFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Product>()
            .Add(It.IsAny<Product>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()));

        var productId = _productService.Create(productFake);

        productId.Should().Be(productFake.Id);
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<Product>().Add(productFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Create_When_ProductAlreadyExists()
    {
        var productFake = ProductFake.ProductFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Product>()
            .Add(It.IsAny<Product>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>())).Returns(true);

        Action act = () => _productService.Create(productFake);

        act.Should().Throw<BadRequestException>($"Product: {productFake.Symbol} are already registered");
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_GetAll_Sucessfully()
    {
        var productFakes = ProductFake.ProductFakers(5);
        var query = Mock.Of<IMultipleResultQuery<Product>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Search(query)).Returns((IList<Product>)productFakes);

        var productsFound = _productService.GetAll();

        productsFound.Should().HaveCount(5);
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Search(query), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_ThereIsNoProducts()
    {
        var query = Mock.Of<IMultipleResultQuery<Product>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Search(query)).Returns(new List<Product>());

        var productsFound = _productService.GetAll();

        productsFound.Should().BeEmpty();
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Search(query), Times.Once());
    }

    [Fact]
    public void Should_GetBySymbol_When_ProductExists()
    {
        var productFake = ProductFake.ProductFaker();
        var query = Mock.Of<IQuery<Product>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleOrDefault(query)).Returns(productFake);

        var productFound = _productService.GetBySymbol(productFake.Symbol);

        productFound.Symbol.Should().Be(productFake.Symbol);
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetBySymbol_When_ProductDoesNotExists()
    {
        var productFake = ProductFake.ProductFaker();
        var query = Mock.Of<IQuery<Product>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleOrDefault(query));

        var productFound = _productService.GetBySymbol(productFake.Symbol);

        productFound.Should().BeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_ProductExists()
    {
        var productFake = ProductFake.ProductFaker();
        var query = Mock.Of<IQuery<Product>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleOrDefault(query)).Returns(productFake);

        var productFound = _productService.GetById(productFake.Id);

        productFound.Id.Should().Be(productFake.Id);
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_ProductDoesNotExists()
    {
        var productFake = ProductFake.ProductFaker();
        var query = Mock.Of<IQuery<Product>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .SingleOrDefault(query));

        var productFound = _productService.GetById(productFake.Id);

        productFound.Should().BeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_Update_When_ProductExists()
    {
        var productFake = ProductFake.ProductFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Product>()
            .Update(It.IsAny<Product>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(true);

        _productService.Update(productFake);

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<Product>()
            .Update(productFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Update_When_ProductDoesNotExists()
    {
        var productFake = ProductFake.ProductFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Product>()
            .Update(It.IsAny<Product>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(false);

        Action act = () => _productService.Update(productFake);

        act.Should().Throw<NotFoundException>($"Product not found for id: {productFake.Id}.");
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_Delete_When_ProductExists()
    {
        var productFake = ProductFake.ProductFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Product>()
            .Remove(It.IsAny<Product>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(true);

        _productService.Delete(productFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<Product>()
            .Remove(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_Delete_When_ProductDoesNotExists()
    {
        var productFake = ProductFake.ProductFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Product>()
            .Remove(It.IsAny<Product>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()))
            .Returns(false);

        Action act = () => _productService.Delete(productFake.Id);

        act.Should().Throw<NotFoundException>($"Product not found for id: {productFake.Id}.");
        _mockRepositoryFactory.Verify(x => x.Repository<Product>()
            .Any(It.IsAny<Expression<Func<Product, bool>>>()), Times.Once());
    }
}

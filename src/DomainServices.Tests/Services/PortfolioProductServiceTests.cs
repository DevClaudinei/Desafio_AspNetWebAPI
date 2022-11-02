using DomainModels.Entities;
using DomainServices.Services;
using DomainServices.Tests.EntitiesFake;
using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Linq.Expressions;

namespace DomainServices.Tests.Services;

public class PortfolioProductServiceTests
{
    private readonly PortfolioProductService _portfolioProductService;
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;

    public PortfolioProductServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();
        _mockRepositoryFactory = new Mock<IRepositoryFactory<ApplicationDbContext>>();
        _portfolioProductService = new PortfolioProductService(
            _mockUnitOfWork.Object,
            _mockRepositoryFactory.Object
        );
    }

    [Fact]
    public void Should_AddProduct_When_ProductDoesNotExistInPortfolioProduct()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Portfolio>>>()
            , It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Product>>>()))
            .Returns(It.IsAny<IQuery<PortfolioProduct>>());
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()));
        _mockUnitOfWork.Setup(x => x.Repository<PortfolioProduct>()
            .Add(It.IsAny<PortfolioProduct>()))
            .Returns(portfolioProductFake);

        // Action
        _portfolioProductService
            .AddProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Portfolio>>>()
            , It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Product>>>()), Times.Never());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<PortfolioProduct>()
            .Add(portfolioProductFake), Times.Never());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_NotAddProduct_When_ProductExistInPortfolioProduct()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Portfolio>>>()
            , It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Product>>>()))
            .Returns(It.IsAny<IQuery<PortfolioProduct>>());
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()))
            .Returns(portfolioProductFake);
        _mockUnitOfWork.Setup(x => x.Repository<PortfolioProduct>()
            .Add(It.IsAny<PortfolioProduct>()));

        // Action
        _portfolioProductService
            .AddProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Portfolio>>>()
            , It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Product>>>())
            , Times.Never());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<PortfolioProduct>()
            .Add(It.IsAny<PortfolioProduct>()), Times.Never());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Never());
    }

    [Fact]
    public void Should_GetAll_When_ThereAreProductsInThePortfolio()
    {
        // Arrange
        var portfoliosFake = PortfolioProductFake.PortfolioProductFakers(2);
        
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<PortfolioProduct>>());
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .Search(It.IsAny<IMultipleResultQuery<PortfolioProduct>>()))
            .Returns((IList<PortfolioProduct>)portfoliosFake);

        // Action
        var portfolioProductFound = _portfolioProductService.GetAll();

        // Assert
        portfolioProductFound.Should().HaveCountGreaterThan(0);

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .Search(It.IsAny<IMultipleResultQuery<PortfolioProduct>>()), Times.Once());
    }

    [Fact]
    public void Should_NotGetAll_When_ThereAreNoProductsInThePortfolio()
    {
        // Arrange
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<PortfolioProduct>>());
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .Search(It.IsAny<IQuery<PortfolioProduct>>()))
            .Returns(new List<PortfolioProduct>());

        // Action
        var portfolioProductFound = _portfolioProductService.GetAll();

        // Assert
        portfolioProductFound.Should().BeEmpty();

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .Search(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_ThereAreProductsInThePortfolio()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Portfolio>>>()
            , It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Product>>>()))
            .Returns(It.IsAny<IQuery<PortfolioProduct>>());
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()))
            .Returns(portfolioProductFake);

        // Act
        var portfolioProductFound = _portfolioProductService
            .GetById(portfolioProductFake.PortfolioId, portfolioProductFake.ProductId);

        // Assert
        portfolioProductFound.Should().NotBeNull();
        
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
    }

    [Fact]
    public void Should_NotGetById_When_ThereAreNoProductsInThePortfolio()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Portfolio>>>()
            , It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Product>>>()))
            .Returns(It.IsAny<IQuery<PortfolioProduct>>());
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()));

        // Act
        var portfolioProductFound = _portfolioProductService
            .GetById(portfolioProductFake.PortfolioId, portfolioProductFake.ProductId);

        // Assert
        portfolioProductFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
    }

    [Fact]
    public void Should_RemoveProduct_Sucessfully()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Portfolio>>>()
            , It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, Product>>>()))
            .Returns(It.IsAny<IQuery<PortfolioProduct>>());
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()))
            .Returns(portfolioProductFake);
        _mockUnitOfWork.Setup(x => x.Repository<PortfolioProduct>()
            .Remove(It.IsAny<PortfolioProduct>()));

        // Act
        _portfolioProductService.RemoveProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<PortfolioProduct>()
            .Remove(It.IsAny<PortfolioProduct>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }
}
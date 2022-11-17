using DomainModels.Entities;
using DomainServices.Services;
using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnitTests.EntitiesFake;

namespace UnitTests.DomainServices;

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
    public void Should_Pass_When_Executing_AddProduct_Because_Product_Does_Not_Exists_In_PortfolioProduct()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Product>>>(),
                It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Portfolio>>>()));

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()));

        _mockUnitOfWork.Setup(x => x.Repository<PortfolioProduct>().Add(It.IsAny<PortfolioProduct>()))
            .Returns(portfolioProductFake);

        // Action
        _portfolioProductService.AddProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<PortfolioProduct>().Add(It.IsAny<PortfolioProduct>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_AddProduct_Because_Product_Exists_In_PortfolioProduct()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Portfolio>>>(),
                It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Product>>>()))
            .Returns(It.IsAny<IQuery<PortfolioProduct>>());

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>())).Returns(portfolioProductFake);

        _mockUnitOfWork.Setup(x => x.Repository<PortfolioProduct>().Add(It.IsAny<PortfolioProduct>()));

        // Action
        _portfolioProductService.AddProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_PortfolioProducts_When_Executing_GetAll()
    {
        // Arrange
        var portfoliosFake = PortfolioProductFake.PortfolioProductFakers(2);

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Portfolio>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Product>>>()))
            .Returns(It.IsAny<IMultipleResultQuery<PortfolioProduct>>());

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .Search(It.IsAny<IMultipleResultQuery<PortfolioProduct>>()))
            .Returns((IList<PortfolioProduct>)portfoliosFake);

        // Action
        var portfolioProductFound = _portfolioProductService.GetAll();

        // Assert
        portfolioProductFound.Should().HaveCount(2);

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery(), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .Search(It.IsAny<IMultipleResultQuery<PortfolioProduct>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_Empty_When_Executing_GetAll()
    {
        // Arrange
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Portfolio>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Product>>>()))
            .Returns(It.IsAny<IMultipleResultQuery<PortfolioProduct>>());

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .Search(It.IsAny<IQuery<PortfolioProduct>>())).Returns(new List<PortfolioProduct>());

        // Action
        var portfolioProductFound = _portfolioProductService.GetAll();

        // Assert
        portfolioProductFound.Should().BeEmpty();

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>().MultipleResultQuery(), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .Search(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_PortfolioProduct_When_Executing_GetById()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Portfolio>>>(),
                It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct,
                Product>>>())).Returns(It.IsAny<IQuery<PortfolioProduct>>());

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>())).Returns(portfolioProductFake);

        // Act
        var portfolioProductFound = _portfolioProductService
            .GetById(portfolioProductFake.PortfolioId, portfolioProductFake.ProductId);

        // Assert
        portfolioProductFound.Id.Should().Be(portfolioProductFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_Null_When_Executing_GetById()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Portfolio>>>(), 
                It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct,
                Product>>>())).Returns(It.IsAny<IQuery<PortfolioProduct>>());

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()));

        // Act
        var portfolioProductFound = _portfolioProductService
            .GetById(portfolioProductFake.PortfolioId, portfolioProductFake.ProductId);

        // Assert
        portfolioProductFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());
    }

    [Fact]
    public void Should_RemoveProduct_Sucessfully()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>,
                IIncludableQueryable<PortfolioProduct, Portfolio>>>(),
                It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct,
                Product>>>())).Returns(It.IsAny<IQuery<PortfolioProduct>>());

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>())).Returns(portfolioProductFake);

        _mockUnitOfWork.Setup(x => x.Repository<PortfolioProduct>().Remove(It.IsAny<PortfolioProduct>()));

        // Act
        _portfolioProductService.RemoveProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(It.IsAny<IQuery<PortfolioProduct>>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<PortfolioProduct>().Remove(It.IsAny<PortfolioProduct>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }
}
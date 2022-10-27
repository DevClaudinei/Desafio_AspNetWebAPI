using DomainModels.Entities;
using DomainServices.Services;
using DomainServices.Tests.EntitiesFake;
using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
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
        var query = Mock.Of<IQuery<PortfolioProduct>>();

        _mockUnitOfWork.Setup(x => x.Repository<PortfolioProduct>()
            .Add(It.IsAny<PortfolioProduct>())).Returns(portfolioProductFake);
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query));

        // Action
        _portfolioProductService
            .AddProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<PortfolioProduct>()
            .Add(portfolioProductFake), Times.Never());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_AddProduct_When_ProductExistInPortfolioProduct()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();
        var query = Mock.Of<IQuery<PortfolioProduct>>();

        _mockUnitOfWork.Setup(x => x.Repository<PortfolioProduct>()
            .Add(It.IsAny<PortfolioProduct>())).Returns(portfolioProductFake);
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query)).Returns(portfolioProductFake);

        // Action
        _portfolioProductService
            .AddProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_ThereAreProductsInThePortfolio()
    {
        // Arrange
        var portfoliosFake = PortfolioProductFake.PortfolioProductFakers(2);
        var query = Mock.Of<IMultipleResultQuery<PortfolioProduct>>();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .Search(query)).Returns((IList<PortfolioProduct>)portfoliosFake);

        // Action
        var portfolioProductFound = _portfolioProductService.GetAll();

        // Assert
        portfolioProductFound.Should().HaveCountGreaterThan(0);
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .Search(query), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_ThereAreNoProductsInThePortfolio()
    {
        // Arrange
        var portfoliosFake = PortfolioProductFake.PortfolioProductFakers(2);
        var query = Mock.Of<IMultipleResultQuery<PortfolioProduct>>();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .Search(query)).Returns(new List<PortfolioProduct>());

        // Action
        var portfolioProductFound = _portfolioProductService.GetAll();

        // Assert
        portfolioProductFound.Should().BeEmpty();
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .Search(query), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_ThereAreProductsInThePortfolio()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();
        var query = Mock.Of<IQuery<PortfolioProduct>>();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query)).Returns(portfolioProductFake);

        // Act
        var portfolioProductFound = _portfolioProductService
            .GetById(portfolioProductFake.PortfolioId, portfolioProductFake.ProductId);

        // Assert
        portfolioProductFound.Should().NotBeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_ThereAreNoProductsInThePortfolio()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();
        var query = Mock.Of<IQuery<PortfolioProduct>>();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query));

        // Act
        var portfolioProductFound = _portfolioProductService
            .GetById(portfolioProductFake.PortfolioId, portfolioProductFake.ProductId);

        // Assert
        portfolioProductFound.Should().BeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_RemoveProduct_Sucessfully()
    {
        // Arrange
        var portfolioProductFake = PortfolioProductFake.PortfolioProductFaker();
        var query = Mock.Of<IQuery<PortfolioProduct>>();

        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query)).Returns(portfolioProductFake);
        _mockUnitOfWork.Setup(x => x.Repository<PortfolioProduct>()
            .Remove(It.IsAny<PortfolioProduct>()));

        // Act
        _portfolioProductService.RemoveProduct(portfolioProductFake.Portfolio, portfolioProductFake.Product);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .Include(It.IsAny<Func<IQueryable<PortfolioProduct>, IIncludableQueryable<PortfolioProduct, object>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>())
            .AndFilter(It.IsAny<Expression<Func<PortfolioProduct, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<PortfolioProduct>()
            .SingleOrDefault(query), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<PortfolioProduct>()
            .Remove(It.IsAny<PortfolioProduct>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }
}
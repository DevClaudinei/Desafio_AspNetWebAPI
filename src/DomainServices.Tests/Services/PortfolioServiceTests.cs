using DomainModels.Entities;
using DomainServices.Exceptions;
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

public class PortfolioServiceTests
{
    private readonly PortfolioService _portfolioService;
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;

    public PortfolioServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();
        _mockRepositoryFactory = new Mock<IRepositoryFactory<ApplicationDbContext>>();
        _portfolioService = new PortfolioService(
            _mockUnitOfWork.Object, _mockRepositoryFactory.Object
        );
    }

    [Fact]
    public void Should_Create_Sucessfully()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Portfolio>()
            .Add(It.IsAny<Portfolio>())).Returns(portfolioFake);

        // Act
        var portfolioId = _portfolioService.Create(portfolioFake);

        // Assert
        portfolioId.Should().Be(portfolioFake.Id);

        _mockUnitOfWork.Verify(x => x.Repository<Portfolio>()
            .Add(portfolioFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_GetTotalBalance_Sucessfully()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance))
            .Returns(It.IsAny<IQuery<Portfolio, decimal>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleOrDefault(It.IsAny<IQuery<Portfolio, decimal>>()));

        // Act
        var totalBalance = _portfolioService.GetTotalBalance(portfolioFake.Id);
 
        // Assert
        totalBalance.Should().Be(portfolioFake.TotalBalance);

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleOrDefault(It.IsAny<IQuery<Portfolio, decimal>>()), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_PortfolioExists()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()))
            .Returns(It.IsAny<IQuery<Portfolio>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .FirstOrDefault(It.IsAny<IQuery<Portfolio>>()))
            .Returns(portfolioFake);

        // Act
        var portfolioFound = _portfolioService.GetById(portfolioFake.Id);
        
        // Assert
        portfolioFound.Id.Should().Be(portfolioFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .FirstOrDefault(It.IsAny<IQuery<Portfolio>>()), Times.Once());
    }

    [Fact]
    public void Should_NotGetById_When_PortfolioDoesNotExists()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()))
            .Returns(It.IsAny<IQuery<Portfolio>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .FirstOrDefault(It.IsAny<IQuery<Portfolio>>()));

        // Act
        var portfolioFound = _portfolioService.GetById(portfolioFake.Id);

        // Assert
        portfolioFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .FirstOrDefault(It.IsAny<IQuery<Portfolio>>()), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_PortfoliosExists()
    {
        // Arrange
        var portfoliosFakes = PortfolioFake.PortfolioFakers(1);
        
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()))
            .Returns(It.IsAny<IQuery<Portfolio>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .Search(It.IsAny<IQuery<Portfolio>>()))
            .Returns((IList<Portfolio>)portfoliosFakes);

        // Act
        var portfoliosFound = _portfolioService.GetAll();

        // Assert
        portfoliosFound.Should().HaveCountGreaterThan(0);

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .Search(It.IsAny<IQuery<Portfolio>>()), Times.Once());
    }

    [Fact]
    public void Should_NotGetAll_When_PortfolioDoesNotExists()
    {
        // Arrange
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
           .MultipleResultQuery()
           .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()))
           .Returns(It.IsAny<IQuery<Portfolio>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .Search(It.IsAny<IQuery<Portfolio>>()));

        // Act
        var portfoliosFound = _portfolioService.GetAll();

        // Assert
        portfoliosFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .Search(It.IsAny<IQuery<Portfolio>>()), Times.Once());
    }

    [Fact]
    public void Should_Update_Sucessfully()
    {   
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Portfolio>()
            .Update(It.IsAny<Portfolio>()));

        // Act
        _portfolioService.Update(portfolioFake);

        // Assert
        _mockUnitOfWork.Verify(x => x.Repository<Portfolio>()
            .Update(portfolioFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Delete_When_PortfolioExists()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance))
            .Returns(It.IsAny<IQuery<Portfolio, decimal>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleOrDefault(It.IsAny<IQuery<Portfolio, decimal>>()));
        _mockUnitOfWork.Setup(x => x.Repository<Portfolio>()
            .Remove(It.IsAny<Expression<Func<Portfolio, bool>>>()));

        // Act
        _portfolioService.Delete(portfolioFake.Id);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleOrDefault(It.IsAny<IQuery<Portfolio, decimal>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<Portfolio>()
            .Remove(It.IsAny<Expression<Func<Portfolio, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_NotDelete_When_PortfolioHasBalanceGranThanZero()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance)).Returns(It.IsAny<IQuery<Portfolio, decimal>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleOrDefault(It.IsAny<IQuery<Portfolio, decimal>>()))
            .Returns(1);
        _mockUnitOfWork.Setup(x => x.Repository<Portfolio>()
            .Remove(It.IsAny<Expression<Func<Portfolio, bool>>>()));

        // Act
        Action act = () => _portfolioService.Delete(portfolioFake.Id);
        
        // Assert
        act.Should()
            .Throw<BadRequestException>($"Unable to delete portfolio, because there is still a balance to withdraw");

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleOrDefault(It.IsAny<IQuery<Portfolio, decimal>>()), Times.Once());
    }

    [Fact]
    public void Should_GetAllByCustomerId_When_PortfolioExists()
    {
        // Arrange
        var portfoliosFake = PortfolioFake.PortfolioFakers(2);
        var portfolioFake = portfoliosFake.ElementAt(1);
        
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()))
            .Returns(It.IsAny<IQuery<Portfolio>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .Search(It.IsAny<IQuery<Portfolio>>()))
            .Returns((IList<Portfolio>)portfoliosFake);

        // Act
        var portfoliosFound = _portfolioService.GetAllByCustomerId(portfolioFake.CustomerId);
        
        // Assert
        portfoliosFound.Should().HaveCount(portfoliosFake.Count());

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .Search(It.IsAny<IQuery<Portfolio>>()), Times.Once());
    }

    [Fact]
    public void Should_NotGetAllByCustomerId_When_PortfolioDoesNotExists()
    {
        // Arrange
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()))
            .Returns(It.IsAny<IMultipleResultQuery<Portfolio>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .Search(It.IsAny<IMultipleResultQuery<Portfolio>>()));

        // Act
        var portfoliosFound = _portfolioService.GetAllByCustomerId(portfolioFake.CustomerId);

        // Assert
        portfoliosFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .Search(It.IsAny<IMultipleResultQuery<Portfolio>>()), Times.Once());
    }
}

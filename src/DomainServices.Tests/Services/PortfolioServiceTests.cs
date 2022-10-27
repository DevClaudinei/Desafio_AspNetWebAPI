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
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;
    private readonly PortfolioService _portfolioService;

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
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Portfolio>()
            .Add(It.IsAny<Portfolio>())).Returns(portfolioFake);

        var portfolioId = _portfolioService.Create(portfolioFake);

        portfolioId.Should().Be(portfolioFake.Id);
        _mockUnitOfWork.Verify(x => x.Repository<Portfolio>()
            .Add(portfolioFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_GetTotalBalance_Sucessfully()
    {
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var query = Mock.Of<IQuery<Portfolio, decimal>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance)).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleOrDefault(query));

        var totalBalance = _portfolioService.GetTotalBalance(portfolioFake.Id);
 
        totalBalance.Should().Be(portfolioFake.TotalBalance);
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_PortfolioExists()
    {
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var query = Mock.Of<IQuery<Portfolio>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>())
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .FirstOrDefault(query)).Returns(portfolioFake);

        var portfolioFound = _portfolioService.GetById(portfolioFake.Id);
        
        portfolioFound.Id.Should().Be(portfolioFake.Id);
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>())
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>()), Times.Once());
            
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .FirstOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_PortfolioDoesNotExists()
    {
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var query = Mock.Of<IQuery<Portfolio>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>())
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .FirstOrDefault(query));

        var portfolioFound = _portfolioService.GetById(portfolioFake.Id);

        portfolioFound.Should().BeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>())
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .FirstOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_PortfoliosExists()
    {
        var portfoliosFakes = PortfolioFake.PortfolioFakers(1);
        var query = Mock.Of<IQuery<Portfolio>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .Search(query)).Returns((IList<Portfolio>)portfoliosFakes);

        var portfoliosFound = _portfolioService.GetAll();

        portfoliosFound.Should().HaveCountGreaterThan(0);
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .Search(query), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_PortfolioDoesNotExists()
    {
        var query = Mock.Of<IQuery<Portfolio>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
           .MultipleResultQuery()
           .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()))
           .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .Search(query));

        var portfoliosFound = _portfolioService.GetAll();

        portfoliosFound.Should().BeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .Search(query), Times.Once());
    }

    [Fact]
    public void Should_Update_Sucessfully()
    {
        var portfolioFake = PortfolioFake.PortfolioFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Portfolio>()
            .Update(It.IsAny<Portfolio>()));

        _portfolioService.Update(portfolioFake);

        _mockUnitOfWork.Verify(x => x.Repository<Portfolio>()
            .Update(portfolioFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Delete_When_PortfolioExists()
    {
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var query = Mock.Of<IQuery<Portfolio, decimal>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance)).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleOrDefault(query));
        _mockUnitOfWork.Setup(x => x.Repository<Portfolio>()
            .Remove(It.IsAny<Expression<Func<Portfolio, bool>>>()));

        _portfolioService.Delete(portfolioFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleOrDefault(query), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<Portfolio>()
            .Remove(It.IsAny<Expression<Func<Portfolio, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_Delete_When_PortfolioHasBalanceGranThanZero()
    {
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var query = Mock.Of<IQuery<Portfolio, decimal>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance)).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .SingleOrDefault(query)).Returns(1);
        _mockUnitOfWork.Setup(x => x.Repository<Portfolio>()
            .Remove(It.IsAny<Expression<Func<Portfolio, bool>>>()));

        Action act = () => _portfolioService.Delete(portfolioFake.Id);
        
        act.Should()
            .Throw<BadRequestException>($"Unable to delete portfolio, because there is still a balance to withdraw");
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>())
            .Select(x => x.TotalBalance), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetAllByCustomerId_When_PortfolioExists()
    {
        var portfolioFake = PortfolioFake.PortfolioFakers(2);
        var query = Mock.Of<IQuery<Portfolio>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>())
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .Search(query)).Returns((IList<Portfolio>)portfolioFake);

        var portfoliosFound = _portfolioService.GetAllByCustomerId(portfolioFake.ElementAt(1).CustomerId);
        
        portfoliosFound.Should().HaveCountGreaterThan(0);
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>())
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .Search(query), Times.Once());
    }

    [Fact]
    public void Should_GetAllByCustomerId_When_PortfolioDoesNotExists()
    {
        var portfolioFake = PortfolioFake.PortfolioFaker();
        var query = Mock.Of<IQuery<Portfolio>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>())
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Portfolio>()
            .Search(query));

        var portfoliosFound = _portfolioService.GetAllByCustomerId(portfolioFake.CustomerId);

        portfoliosFound.Should().BeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .MultipleResultQuery()
            .Include(It.IsAny<Func<IQueryable<Portfolio>, IIncludableQueryable<Portfolio, object>>>())
            .AndFilter(It.IsAny<Expression<Func<Portfolio, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Portfolio>()
            .Search(query), Times.Once());
    }
}

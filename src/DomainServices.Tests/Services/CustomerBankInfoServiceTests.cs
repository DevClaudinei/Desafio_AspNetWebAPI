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

public class CustomerBankInfoServiceTests
{
    private readonly CustomerBankInfoService _customerBankInfoService;
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;

    public CustomerBankInfoServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();
        _mockRepositoryFactory = new Mock<IRepositoryFactory<ApplicationDbContext>>();
        _customerBankInfoService = new CustomerBankInfoService(_mockUnitOfWork.Object, _mockRepositoryFactory.Object);
    }

    [Fact]
    public void Should_Create_Sucessfully()
    {
        // Arrange
        var customerBankInfoFake = CustomerFake.CustomerFaker();
        
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Add(It.IsAny<CustomerBankInfo>()));

        // Act
        _customerBankInfoService.Create(customerBankInfoFake.Id);

        // Assert
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Exactly(1));
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_CustomerBankInfoExist()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()))
            .Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoService.GetById(bankInfoFake.Id);
        
        // Assert
        bankInfoFound.Id.Should().Be(bankInfoFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_CustomerBankInfoDoesNotExist()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        var bankInfoFound = _customerBankInfoService.GetById(bankInfoFake.Id);

        // Assert
        bankInfoFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_GetIdByCustomerId_When_CustomerExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo, long>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, long>>()))
            .Returns(bankInfoFake.CustomerId);

        // Act
        var bankInfoId = _customerBankInfoService.GetIdByCustomerId(bankInfoFake.CustomerId);
        
        // Assert
        bankInfoId.Should().Be(bankInfoFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, long>>()), Times.Once());
    }

    [Fact]
    public void Should_GetIdByCustomerId_When_CustomerDoesNotExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, long>>()));
        
        // Act
        var bankInfoId = _customerBankInfoService.GetIdByCustomerId(bankInfoFake.CustomerId);

        // Assert
        bankInfoId.Should().Be(0);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, long>>()), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_CustomerBankInfoExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFakers(5);
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .Search(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>()))
            .Returns((IList<CustomerBankInfo>)bankInfoFake);

        // Act
        var bankInfoFakesFound = _customerBankInfoService.GetAll();

        // Assert
        bankInfoFakesFound.Should().HaveCount(5);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .MultipleResultQuery(), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_CustomerBankInfoDoesNotExists()
    {
        // Arrange
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .Search(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>()))
            .Returns(new List<CustomerBankInfo>());

        // Act
        var bankInfoFakesFound = _customerBankInfoService.GetAll();

        // Assert
        bankInfoFakesFound.Should().BeEmpty();
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .Search(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_GetByAccount_When_CustomerBankInfoExist()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()))
            .Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoService.GetByAccount(bankInfoFake.Account);

        // Assert
        bankInfoFound.Account.Should().Be(bankInfoFake.Account);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_GetByAccount_When_CustomerBankInfoDoesNotExist()
    {
        // Arrange
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        var customerBankInfoFound = _customerBankInfoService.GetByAccount(customerBankInfoFake.Account);

        // Assert
        customerBankInfoFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_GetAccountBalanceById_Sucessfully()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())
            .Select(x => x.AccountBalance)).Returns(It.IsAny<IQuery<CustomerBankInfo, decimal>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, decimal>>()));

        // Act
        var accountBalance = _customerBankInfoService.GetAccountBalanceById(bankInfoFake.Id);

        // Assert
        accountBalance.Should().Be(0);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())
            .Select(x => x.AccountBalance), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, decimal>>()), Times.Once());
    }

    [Fact]
    public void Should_Deposit_When_CustomerBankInfoExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;
        
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Update(It.IsAny<CustomerBankInfo>()));
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()))
            .Returns(bankInfoFake);

        // Act
        _customerBankInfoService.Deposit(bankInfoFake.Id, amount);

        // Assert
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>()
            .Update(bankInfoFake, x => x.AccountBalance), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Exactly(1));
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Deposit_When_CustomerBankInfoDoesNotExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;
        
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Update(It.IsAny<CustomerBankInfo>()));
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        Action act = () => _customerBankInfoService.Deposit(bankInfoFake.Id, amount);

        // Assert
        act.Should().Throw<NotFoundException>($"CustomerBankInfo for id: {bankInfoFake.Id} not found.");

        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_Withdraw_When_CustomerBankInfoExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()))
            .Returns(bankInfoFake);
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Update(It.IsAny<CustomerBankInfo>()));

        // Act
        _customerBankInfoService.Withdraw(bankInfoFake.Id, amount);

        // Assert
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Exactly(1));
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>()
            .Update(bankInfoFake, x => x.AccountBalance), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Withdraw_When_CustomerBankInfoDoesNotExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;
        
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Update(It.IsAny<CustomerBankInfo>()));
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        Action act = () => _customerBankInfoService.Withdraw(bankInfoFake.Id, amount);

        // Assert
        act.Should().Throw<NotFoundException>($"CustomerBankInfo for id: {bankInfoFake.Id} not found.");

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Exactly(1));
    }

    [Fact]
    public void Should_GetByCustomerId_When_CustomerExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>())).Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoService.GetByCustomerId(bankInfoFake.CustomerId);

        // Assert
        bankInfoFound.Should().BeEquivalentTo(bankInfoFake);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_GetByCustomerId_When_CustomerDoesNotExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        var bankInfoFound = _customerBankInfoService.GetByCustomerId(bankInfoFake.CustomerId);

        // Assert
        bankInfoFound.Should().BeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }
}

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
using UnitTests.EntitiesFake.CustomerBankInfos;
using UnitTests.EntitiesFake.Customers;

namespace UnitTests.DomainServices;

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
    public void Should_Create_CustomerBankInfo_Sucessfully()
    {
        // Arrange
        var customerBankInfoFake = CustomerFake.CustomerFaker();

        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>().Add(It.IsAny<CustomerBankInfo>()));

        // Act
        _customerBankInfoService.Create(customerBankInfoFake.Id);

        // Assert
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Once());

        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetById()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(x => x.Id.Equals(bankInfoFake.Id))).Returns(It.IsAny<IQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>())).Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoService.GetById(bankInfoFake.Id);

        // Assert
        bankInfoFound.Id.Should().Be(bankInfoFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetById()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())).Returns(It.IsAny<IQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        var bankInfoFound = _customerBankInfoService.GetById(bankInfoFake.Id);

        // Assert
        bankInfoFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetIdByCustomerId()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery().Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo, long>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, long>>())).Returns(bankInfoFake.CustomerId);

        // Act
        var bankInfoId = _customerBankInfoService.GetIdByCustomerId(bankInfoFake.CustomerId);

        // Assert
        bankInfoId.Should().Be(bankInfoFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery().Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, long>>()), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetIdByCustomerId()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery().Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()));

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, long>>()));

        // Act
        var bankInfoId = _customerBankInfoService.GetIdByCustomerId(bankInfoFake.CustomerId);

        // Assert
        bankInfoId.Should().Be(0);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery().Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, long>>()), Times.Once());
    }

    [Fact]
    public void Should_Return_CustomerBankInfos_When_Executing_GetAll()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFakers(5);

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .Search(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>())).Returns((IList<CustomerBankInfo>)bankInfoFake);

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
    public void Should_Return_Empty_When_Executing_GetAll()
    {
        // Arrange
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .Search(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>())).Returns(new List<CustomerBankInfo>());

        // Act
        var bankInfoFakesFound = _customerBankInfoService.GetAll();

        // Assert
        bankInfoFakesFound.Should().BeEmpty();
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().MultipleResultQuery(), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .Search(It.IsAny<IMultipleResultQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetByAccount()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>())).Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoService.GetByAccount(bankInfoFake.Account);

        // Assert
        bankInfoFound.Account.Should().Be(bankInfoFake.Account);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetByAccount()
    {
        // Arrange
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        var customerBankInfoFound = _customerBankInfoService.GetByAccount(customerBankInfoFake.Account);

        // Assert
        customerBankInfoFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_GetAccountBalanceById_On_CustomerBankInfo_Sucessfully()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())
            .Select(x => x.AccountBalance)).Returns(It.IsAny<IQuery<CustomerBankInfo, decimal>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, decimal>>()));

        // Act
        var accountBalance = _customerBankInfoService.GetAccountBalanceById(bankInfoFake.Id);

        // Assert
        accountBalance.Should().Be(0);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())
            .Select(x => x.AccountBalance), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo, decimal>>()), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_Deposit_On_CustomerBankInfo()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;

        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>().Update(It.IsAny<CustomerBankInfo>()));

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>())).Returns(bankInfoFake);

        // Act
        _customerBankInfoService.Deposit(bankInfoFake.Id, amount);

        // Assert
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>()
            .Update(bankInfoFake, x => x.AccountBalance), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Once());

        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_Deposit_On_CustomerBankInfo()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;

        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>().Update(It.IsAny<CustomerBankInfo>()));

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        Action act = () => _customerBankInfoService.Deposit(bankInfoFake.Id, amount);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"CustomerBankInfo for id: {bankInfoFake.Id} not found.");

        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_Withdraw_From_CustomerBankInfo()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>())).Returns(bankInfoFake);

        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>().Update(It.IsAny<CustomerBankInfo>()));

        // Act
        _customerBankInfoService.Withdraw(bankInfoFake.Id, amount);

        // Assert
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>()
            .Update(bankInfoFake, x => x.AccountBalance), Times.Once());

        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_Withdraw_From_CustomerBankInfo()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;

        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>().Update(It.IsAny<CustomerBankInfo>()));

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        Action act = () => _customerBankInfoService.Withdraw(bankInfoFake.Id, amount);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"CustomerBankInfo for id: {bankInfoFake.Id} not found.");

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());

        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetByCustomerId()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>())).Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoService.GetByCustomerId(bankInfoFake.CustomerId);

        // Assert
        bankInfoFound.Should().BeEquivalentTo(bankInfoFake);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetByCustomerId()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(It.IsAny<IQuery<CustomerBankInfo>>());

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()));

        // Act
        var bankInfoFound = _customerBankInfoService.GetByCustomerId(bankInfoFake.CustomerId);

        // Assert
        bankInfoFound.Should().BeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>().SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(It.IsAny<IQuery<CustomerBankInfo>>()), Times.Once());
    }
}
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
    public void Should_CreateCustomerBankInfo_Sucessfully()
    {
        var customerBankInfoFake = CustomerFake.CustomerFaker();
        
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Add(It.IsAny<CustomerBankInfo>()));

        _customerBankInfoService.Create(customerBankInfoFake.Id);

        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Exactly(1));
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_ReturnCustomerBankInfo_When_IdExist()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query)).Returns(customerBankInfoFake);

        var customerBankInfoFound = _customerBankInfoService.GetById(customerBankInfoFake.Id);
        
        customerBankInfoFound.Id.Should().Be(1);
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_ReturnCustomerBankInfo_When_IdDoesNotExist()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query));

        var customerBankInfoFound = _customerBankInfoService.GetById(customerBankInfoFake.Id);

        customerBankInfoFound.Should().BeNull();
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_ReturnCustomerId_When_CustomerExists()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var query = Mock.Of<IQuery<CustomerBankInfo, long>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query)).Returns(customerBankInfoFake.CustomerId);

        var customerBankInfoId = _customerBankInfoService.GetIdByCustomerId(customerBankInfoFake.CustomerId);
        
        customerBankInfoId.Should().Be(customerBankInfoFake.Id);
    }

    [Fact]
    public void Should_ReturnCustomerId_When_CustomerDoesNotExists()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var query = Mock.Of<IQuery<CustomerBankInfo, long>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .Select(x => x.Id)
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query));

        var customerBankInfoId = _customerBankInfoService.GetIdByCustomerId(customerBankInfoFake.CustomerId);
        customerBankInfoId.Should().Be(0);
    }

    [Fact]
    public void Should_ReturnAllCustomerBankInfos_Sucessfully()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFakers(5);
        var query = Mock.Of<IMultipleResultQuery<CustomerBankInfo>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .Search(query)).Returns((IList<CustomerBankInfo>)customerBankInfoFake);

        var customerBankInfoFakesFound = _customerBankInfoService.GetAll();
        customerBankInfoFakesFound.Should().HaveCount(5);

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .MultipleResultQuery(), Times.Once());
    }

    [Fact]
    public void Should_NoReturns_When_NoCustomerBankInfosRegistered()
    {
        var query = Mock.Of<IMultipleResultQuery<CustomerBankInfo>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .Search(query)).Returns(new List<CustomerBankInfo>());

        var customerBankInfoFakesFound = _customerBankInfoService.GetAll();
        customerBankInfoFakesFound.Should().BeEmpty();
    }

    [Fact]
    public void Should_ReturnCustomerBankInfo_When_AccountExist()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query)).Returns(customerBankInfoFake);

        var customerBankInfoFound = _customerBankInfoService.GetByAccount(customerBankInfoFake.Account);
        customerBankInfoFound.Account.Should().NotBeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_ReturnCustomerBankInfo_When_AccountDoesNotExist()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query));

        var customerBankInfoFound = _customerBankInfoService.GetByAccount(customerBankInfoFake.Account);
        customerBankInfoFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
           .SingleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_ReturnTotalBalance_Sucessfully()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var query = Mock.Of<IQuery<CustomerBankInfo, decimal>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>())
            .Select(x => x.AccountBalance)).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query));

        var accountBalance = _customerBankInfoService.GetAccountBalanceById(customerBankInfoFake.Id);
        accountBalance.Should().Be(0);
    }

    [Fact]
    public void Should_RealizeDeposit_When_CustomerBankInfoExists()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Update(It.IsAny<CustomerBankInfo>()));
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Remove(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query)).Returns(customerBankInfoFake);

        _customerBankInfoService.Deposit(customerBankInfoFake.Id, amount);
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Exactly(1));
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>().Update(customerBankInfoFake, x => x.AccountBalance), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_RealizeDeposit_When_CustomerBankInfoDoesNotExists()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Update(It.IsAny<CustomerBankInfo>()));
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Remove(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query));

        Action act = () => _customerBankInfoService.Deposit(customerBankInfoFake.Id, amount);
        act.Should().Throw<NotFoundException>($"CustomerBankInfo for id: {customerBankInfoFake.Id} not found.");
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Exactly(1));
    }

    [Fact]
    public void Should_RealizeWithdraw_When_CustomerBankInfoExists()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Update(It.IsAny<CustomerBankInfo>()));
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Remove(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query)).Returns(customerBankInfoFake);

        _customerBankInfoService.Withdraw(customerBankInfoFake.Id, amount);
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Exactly(1));
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>().Update(customerBankInfoFake, x => x.AccountBalance), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_RealizeWithdraw_When_CustomerBankInfoDoesNotExists()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Update(It.IsAny<CustomerBankInfo>()));
        _mockUnitOfWork.Setup(x => x.Repository<CustomerBankInfo>()
            .Remove(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query));

        Action act = () => _customerBankInfoService.Withdraw(customerBankInfoFake.Id, amount);
        act.Should().Throw<NotFoundException>($"CustomerBankInfo for id: {customerBankInfoFake.Id} not found.");
        _mockUnitOfWork.Verify(x => x.Repository<CustomerBankInfo>(), Times.Exactly(1));
    }

    [Fact]
    public void Should_ReturnCustomerBankInfo_When_CustomerExists()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query)).Returns(customerBankInfoFake);

        var customerBankInfo = _customerBankInfoService.GetByCustomerId(customerBankInfoFake.CustomerId);
        customerBankInfo.Should().BeEquivalentTo(customerBankInfoFake);
    }

    [Fact]
    public void Should_ReturnCustomerBankInfo_When_CustomerDoesNotExists()
    {
        var customerBankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var query = Mock.Of<IQuery<CustomerBankInfo>>();

        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<CustomerBankInfo, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<CustomerBankInfo>()
            .SingleOrDefault(query));

        var customerBankInfo = _customerBankInfoService.GetByCustomerId(customerBankInfoFake.CustomerId);
        customerBankInfo.Should().BeNull();
    }
}

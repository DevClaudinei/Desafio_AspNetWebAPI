using AppServices.Profiles;
using AppServices.Services;
using AutoMapper;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using UnitTests.EntitiesFake.CustomerBankInfos;

namespace UnitTests.AppServices;

public class CustomerBankInfoAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly CustomerBankInfoAppService _customerBankInfoAppService;
    private readonly Mock<ICustomerBankInfoService> _customerBankInfoService;

    public CustomerBankInfoAppServiceTests()
    {
        var config = new MapperConfiguration(opt =>
        {
            opt.AddProfile(new CustomerBankInfoProfile());
        });
        _mapper = config.CreateMapper();
        _customerBankInfoService = new Mock<ICustomerBankInfoService>();
        _customerBankInfoAppService = new CustomerBankInfoAppService(
            _customerBankInfoService.Object, _mapper
        );
    }

    [Fact]
    public void Should_Return_CustomerBankInfos_When_Executing_GetAll()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFakers(2);

        _customerBankInfoService.Setup(x => x.GetAll()).Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoAppService.GetAll();

        // Assert
        bankInfoFound.Should().HaveCount(2);

        _customerBankInfoService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_Return_Empty_When_Executing_GetAll()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFakers(2);

        _customerBankInfoService.Setup(x => x.GetAll());

        // Act
        var bankInfoFound = _customerBankInfoAppService.GetAll();

        // Assert
        bankInfoFound.Should().BeEmpty();

        _customerBankInfoService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetByAccount()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetByAccount(bankInfoFake.Account)).Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoAppService.GetByAccount(bankInfoFake.Account);

        // Assert
        bankInfoFound.Account.Should().Be(bankInfoFake.Account);

        _customerBankInfoService.Verify(x => x.GetByAccount(bankInfoFake.Account), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetByAccount()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetByAccount(bankInfoFake.Account));

        // Act
        Action act = () => _customerBankInfoAppService.GetByAccount(bankInfoFake.Account);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"CustomerBankInfo for account: {bankInfoFake.Account} could not be found.");

        _customerBankInfoService.Verify(x => x.GetByAccount(bankInfoFake.Account), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetById()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetById(bankInfoFake.Id)).Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoAppService.GetById(bankInfoFake.Id);

        // Assert
        bankInfoFound.Id.Should().Be(bankInfoFake.Id);

        _customerBankInfoService.Verify(x => x.GetById(bankInfoFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetById()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetById(bankInfoFake.Id));

        // Act
        Action act = () => _customerBankInfoAppService.GetById(bankInfoFake.Id);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"CustomerBankInfo for id: {bankInfoFake.Id} not found.");

        _customerBankInfoService.Verify(x => x.GetById(bankInfoFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Return_BankInfoId_When_Executing_GetCustomerBankInfoId()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetIdByCustomerId(bankInfoFake.CustomerId)).Returns(bankInfoFake.Id);

        // Act
        var bankInfoId = _customerBankInfoAppService.GetCustomerBankInfoId(bankInfoFake.CustomerId);

        // Assert
        bankInfoId.Should().Be(bankInfoFake.Id);

        _customerBankInfoService.Verify(x => x.GetIdByCustomerId(bankInfoFake.CustomerId), Times.Once());
    }

    [Fact]
    public void Should_Return_Zero_When_Executing_GetCustomerBankInfoId()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetIdByCustomerId(bankInfoFake.CustomerId));

        // Act
        var bankInfoId = _customerBankInfoAppService.GetCustomerBankInfoId(bankInfoFake.CustomerId);

        // Assert
        bankInfoId.Should().Be(0);

        _customerBankInfoService.Verify(x => x.GetIdByCustomerId(bankInfoFake.CustomerId), Times.Once());
    }

    [Fact]
    public void Should_Deposit_On_CustomerBankInfo_Sucessfully()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 1;

        _customerBankInfoService.Setup(x => x.Deposit(bankInfoFake.Id, amount));

        // Act
        _customerBankInfoAppService.Deposit(bankInfoFake.Id, amount);

        // Assert
        _customerBankInfoService.Verify(x => x.Deposit(bankInfoFake.Id, amount), Times.Once());
    }

    [Fact]
    public void Should_Withdraw_From_CustomerBankInfo_Sucessfully()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 1;

        _customerBankInfoService.Setup(x => x.Withdraw(bankInfoFake.Id, amount));

        // Act
        _customerBankInfoAppService.Withdraw(bankInfoFake.Id, amount);

        // Assert
        _customerBankInfoService.Verify(x => x.Withdraw(bankInfoFake.Id, amount), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_CanWithdrawAmount()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;

        _customerBankInfoService.Setup(x => x.GetAccountBalanceById(bankInfoFake.Id)).Returns(bankInfoFake.AccountBalance);

        // Act
        _customerBankInfoAppService.CanWithdrawAmountFromAccountBalance(amount, bankInfoFake.Id);

        // Assert
        _customerBankInfoService.Verify(x => x.GetAccountBalanceById(bankInfoFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_CanWithdrawAmount_Because_Amount_Greater_Than_AccountBalance()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 1;

        _customerBankInfoService.Setup(x => x.GetAccountBalanceById(It.IsAny<long>())).Returns(It.IsAny<decimal>());

        // Act
        Action act = () => _customerBankInfoAppService.CanWithdrawAmountFromAccountBalance(amount, bankInfoFake.Id);

        // Assert
        act.Should().ThrowExactly<BadRequestException>($"Insufficient balance to invest.");

        _customerBankInfoService.Verify(x => x.GetAccountBalanceById(bankInfoFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Executing_GetByCustomerId()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetByCustomerId(bankInfoFake.CustomerId)).Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoAppService.GetByCustomerId(bankInfoFake.Id);

        // Assert
        bankInfoFound.Id.Should().Be(bankInfoFake.Id);

        _customerBankInfoService.Verify(x => x.GetByCustomerId(bankInfoFake.CustomerId), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Executing_GetCustomerId()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetByCustomerId(It.IsAny<long>()));

        // Act
        Action act = () => _customerBankInfoAppService.GetByCustomerId(bankInfoFake.Id);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"CustomerBankInfo for CustomerId: {bankInfoFake.Id} could not be found.");

        _customerBankInfoService.Verify(x => x.GetByCustomerId(It.IsAny<long>()), Times.Once());
    }

    [Fact]
    public void Should_Create_CustomerBankIfo_Sucessfully()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.Create(It.IsAny<long>()));

        // Act
        _customerBankInfoAppService.Create(bankInfoFake.CustomerId);

        // Assert
        _customerBankInfoService.Verify(x => x.Create(It.IsAny<long>()), Times.Once());
    }
}
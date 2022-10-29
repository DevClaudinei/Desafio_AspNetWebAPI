using AppServices.Profiles;
using AppServices.Services;
using AutoMapper;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using DomainServices.Tests.EntitiesFake;
using FluentAssertions;
using Moq;

namespace AppServices.Tests.Services;

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
    public void Should_GetAll_When_CustomerBankInfoExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFakers(2);
        
        _customerBankInfoService.Setup(x => x.GetAll()).Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoAppService.GetAll();

        // Assert
        bankInfoFound.Should().HaveCountGreaterThan(0);
        _customerBankInfoService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_CustomerBankInfoDoesNotExists()
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
    public void Should_GetByAccount_When_CustomerExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetByAccount(bankInfoFake.Account))
            .Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoAppService.GetByAccount(bankInfoFake.Account);

        // Assert
        bankInfoFound.Account.Should().Be(bankInfoFake.Account);
        _customerBankInfoService.Verify(x => x.GetByAccount(bankInfoFake.Account), Times.Once());
    }

    [Fact]
    public void Should_GetByAccount_When_CustomerDoesNotExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetByAccount(bankInfoFake.Account));

        // Act
        Action act = () => _customerBankInfoAppService.GetByAccount(bankInfoFake.Account);

        // Assert
        act.Should().Throw<NotFoundException>($"CustomerBankInfo for account: {bankInfoFake.Account} was not found.");
        _customerBankInfoService.Verify(x => x.GetByAccount(bankInfoFake.Account), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_CustomerExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetById(bankInfoFake.Id))
            .Returns(bankInfoFake);

        // Act
        var bankInfoFound = _customerBankInfoAppService.GetById(bankInfoFake.Id);

        // Assert
        bankInfoFound.Id.Should().Be(bankInfoFake.Id);
        _customerBankInfoService.Verify(x => x.GetById(bankInfoFake.Id), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_CustomerDoesNotExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetById(bankInfoFake.Id));

        // Act
        Action act = () => _customerBankInfoAppService.GetById(bankInfoFake.Id);

        // Assert
        act.Should().Throw<NotFoundException>($"CustomerBankInfo for id: {bankInfoFake.Id} not found.");
        _customerBankInfoService.Verify(x => x.GetById(bankInfoFake.Id), Times.Once());
    }

    [Fact]
    public void Should_GetCustomerBankInfoId_When_CustomerExists()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();

        _customerBankInfoService.Setup(x => x.GetIdByCustomerId(bankInfoFake.CustomerId))
            .Returns(bankInfoFake.Id);

        // Act
        var bankInfoId = _customerBankInfoAppService.GetCustomerBankInfoId(bankInfoFake.CustomerId);

        // Assert
        bankInfoId.Should().Be(bankInfoFake.Id);
        _customerBankInfoService.Verify(x => x.GetIdByCustomerId(bankInfoFake.CustomerId), Times.Once());
    }

    [Fact]
    public void Should_GetCustomerBankInfoId_When_CustomerDoesNotExists()
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
    public void Should_Deposit_Sucessfully()
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
    public void Should_Withdraw_Sucessfully()
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
    public void Should_CanWithdrawAmountFromAccountBalance_When_AmountLessThanAccountBalance()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 0;

        _customerBankInfoService.Setup(x => x.GetAccountBalanceById(bankInfoFake.Id))
            .Returns(bankInfoFake.AccountBalance);

        // Act
        _customerBankInfoAppService.CanWithdrawAmountFromAccountBalance(amount, bankInfoFake.Id);

        // Assert
        _customerBankInfoService.Verify(x => x.GetAccountBalanceById(bankInfoFake.Id), Times.Once());
    }

    [Fact]
    public void Should_CanWithdrawAmountFromAccountBalance_When_AmountGranThanAccountBalance()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoFake.CustomerBankInfoFaker();
        var amount = 1;

        _customerBankInfoService.Setup(x => x.GetAccountBalanceById(bankInfoFake.Id))
            .Returns(bankInfoFake.AccountBalance);

        // Act
        Action act = () => _customerBankInfoAppService.CanWithdrawAmountFromAccountBalance(amount, bankInfoFake.Id);

        // Assert
        act.Should().Throw<BadRequestException>($"Insufficient balance to invest.");
        _customerBankInfoService.Verify(x => x.GetAccountBalanceById(bankInfoFake.Id), Times.Once());
    }
}
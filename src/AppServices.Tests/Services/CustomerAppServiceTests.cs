using AppServices.Profiles;
using AppServices.Services;
using AppServices.Services.Interfaces;
using AppServices.Tests.ModelsFake.Customer;
using AppServices.Tests.ModelsFake.CustomerBankInfo;
using AppServices.Tests.Portfolio;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services;
using DomainServices.Tests.EntitiesFake;
using FluentAssertions;
using Moq;

namespace AppServices.Tests.Services;

public class CustomerAppServiceTests
{
    private readonly IMapper _mapper;
    private readonly CustomerAppService _customerAppService;
    private readonly Mock<ICustomerService> _customerService;
    private readonly Mock<IPortfolioAppService> _portfolioAppService;
    private readonly Mock<ICustomerBankInfoAppService> _customerBankInfoAppService;

    public CustomerAppServiceTests()
    {
        var config = new MapperConfiguration(opt =>
        {
            opt.AddProfile(new CustomerProfile());
        });
        _mapper = config.CreateMapper();
        _customerService = new Mock<ICustomerService>();
        _portfolioAppService = new Mock<IPortfolioAppService>();
        _customerBankInfoAppService = new Mock<ICustomerBankInfoAppService>();
        _customerAppService = new CustomerAppService(
            _mapper,
            _customerService.Object,
            _portfolioAppService.Object,
            _customerBankInfoAppService.Object
        );
    }

    [Fact]
    public void Should_Create_Sucessfully()
    {
        // Arrange
        var createCustomerRequest = CreateCustomerModel.CustomerFaker();
        var id = 1L;

        _customerService.Setup(x => x.CreateCustomer(It.IsAny<Customer>()))
            .Returns(id);
        _customerBankInfoAppService.Setup(x => x.Create(id));
     
        // Act
        var customerId = _customerAppService.Create(createCustomerRequest);

        // Assert
        customerId.Should().Be(id);

        _customerService.Verify(x => x.CreateCustomer(It.IsAny<Customer>()), Times.Once());
        _customerBankInfoAppService.Verify(x => x.Create(id), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Run_GetAll()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFakers(2);

        _customerService.Setup(x => x.GetAll()).Returns(customerFake);

        // Act
        var customersFound = _customerAppService.GetAll();

        // Assert
        customersFound.Count().Should().Be(2);
        _customerService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Run_GetAll()
    {
        // Arrange
        _customerService.Setup(x => x.GetAll()).Returns(new List<Customer>());

        // Act
        var customersFound = _customerAppService.GetAll();

        // Assert
        customersFound.Should().BeEmpty();
        _customerService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Run_GetById()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();

        _customerService.Setup(x => x.GetById(customerFake.Id)).Returns(customerFake);

        // Act
        var customerResultFound = _customerAppService.GetById(customerFake.Id);

        // Assert
        customerResultFound.Id.Should().Be(customerFake.Id);
        _customerService.Verify(x => x.GetById(customerFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Run_GetById()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();

        _customerService.Setup(x => x.GetById(customerFake.Id));

        // Act
        Action act = () => _customerAppService.GetById(customerFake.Id);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Customer for Id: {customerFake.Id} was not found.");

        _customerService.Verify(x => x.GetById(customerFake.Id), Times.Once());
    }

    [Fact]
    public void Should_Pass_When_Run_GetByFullName()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFakers(1);
        var customer = customerFake.First();

        _customerService.Setup(x => x.GetAllByFullName(customer.FullName))
            .Returns((IList<Customer>)customerFake);

        // Act
        var customerFound = _customerAppService.GetByName(customer.FullName);

        // Assert
        customerFound.Should().NotBeNull();
        _customerService.Verify(x => x.GetAllByFullName(customer.FullName), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Run_GetByFullName()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();
        
        _customerService.Setup(x => x.GetAllByFullName(customerFake.FullName));

        // Act
        Action act = () => _customerAppService.GetByName(customerFake.FullName);

        // Assert
        act.Should().ThrowExactly<NotFoundException>($"Customer for Id: {customerFake.FullName} was not found.");

        _customerService.Verify(x => x.GetAllByFullName(customerFake.FullName), Times.Once());
    }

    [Fact]
    public void Should_Update_Sucessfully()
    {
        // Arrange
        var updateCustomer = UpdateCustomerModel.CustomerFaker();
        var customer = CustomerFake.CustomerFaker();
        var id = 1L;

        _customerService.Setup(x => x.Update(id, customer));

        // Act
        _customerAppService.Update(id, updateCustomer);

        // Arrange
        _customerService.Verify(x => x.Update(id, customer), Times.Never());
    }

    [Fact]
    public void Should_Pass_When_Run_Delete()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoResponseModel.BankInfoFake();
        var portfolioFake = PortfolioResponseModel.PortfolioFake(2);

        _customerBankInfoAppService.Setup(x => x.GetByCustomerId(bankInfoFake.CustomerId))
            .Returns(bankInfoFake);
        _portfolioAppService.Setup(x => x.GetAllByCustomerId(bankInfoFake.CustomerId))
            .Returns(portfolioFake);

        // Act
        _customerAppService.Delete(bankInfoFake.CustomerId);

        // Assert
        _customerBankInfoAppService.Verify(x => x.GetByCustomerId(bankInfoFake.CustomerId), Times.Once());
        _portfolioAppService.Verify(x => x.GetAllByCustomerId(bankInfoFake.CustomerId), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Run_Delete_Because_AccountBalanceGranThanZero()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoResponseModel.BankInfoFake();
        bankInfoFake.AccountBalance = 1;
        var portfolioFake = PortfolioResponseModel.PortfolioFake(2);

        _customerBankInfoAppService.Setup(x => x.GetByCustomerId(bankInfoFake.CustomerId))
            .Returns(bankInfoFake);
        _portfolioAppService.Setup(x => x.GetAllByCustomerId(bankInfoFake.CustomerId))
            .Returns(portfolioFake);

        // Act
        Action act = () => _customerAppService.Delete(bankInfoFake.CustomerId);

        // Assert
        act.Should()
            .Throw<BadRequestException>("Customer needs to Withdraw the account balance before being deleted.");

        _customerBankInfoAppService.Verify(x => x.GetByCustomerId(bankInfoFake.CustomerId), Times.Once());
        _portfolioAppService.Verify(x => x.GetAllByCustomerId(bankInfoFake.CustomerId), Times.Once());
    }

    [Fact]
    public void Should_Fail_When_Run_Delete_Because_AnyTotalBalanceGranThanZero()
    {
        // Arrange
        var bankInfoFake = CustomerBankInfoResponseModel.BankInfoFake();
        var portfolioFake = PortfolioResponseModel.PortfolioFake(2);
        portfolioFake.ElementAt(1).TotalBalance = 1;

        _customerBankInfoAppService.Setup(x => x.GetByCustomerId(bankInfoFake.CustomerId))
            .Returns(bankInfoFake);
        _portfolioAppService.Setup(x => x.GetAllByCustomerId(bankInfoFake.CustomerId))
            .Returns(portfolioFake);

        // Act
        Action act = () => _customerAppService.Delete(bankInfoFake.CustomerId);

        // Assert
        act.Should()
            .Throw<BadRequestException>("Customer needs to withdraw the balance from the portfolio before being deleted.");

        _customerBankInfoAppService.Verify(x => x.GetByCustomerId(bankInfoFake.CustomerId), Times.Once());
        _portfolioAppService.Verify(x => x.GetAllByCustomerId(bankInfoFake.CustomerId), Times.Once());
    }
}
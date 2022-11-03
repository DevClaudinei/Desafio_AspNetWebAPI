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
        var createdCustomer = _customerAppService.Create(createCustomerRequest);

        // Assert
        createdCustomer.Should().Be(id);

        _customerService.Verify(x => x.CreateCustomer(It.IsAny<Customer>()), Times.Once());
        _customerBankInfoAppService.Verify(x => x.Create(id), Times.Once());
    }

    [Fact]
    public void Should_Get_When_CustomersExists()
    {
        // Arrange
        var customersResult = CustomerResponseModel.CustomerFakers(2);
        var customerFake = CustomerFake.CustomerFakers(2);

        _customerService.Setup(x => x.GetAll()).Returns(customerFake);

        // Act
        var customersFound = _customerAppService.Get();

        // Assert
        customersFound.Should().HaveCountGreaterThanOrEqualTo(0);
        _customerService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_Get_When_CustomersDoesNotExists()
    {
        // Arrange
        var customerFake = new List<Customer>();

        _customerService.Setup(x => x.GetAll()).Returns(customerFake);

        // Act
        var customersFound = _customerAppService.Get();

        // Assert
        customersFound.Should().BeEmpty();
        _customerService.Verify(x => x.GetAll(), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_CustomerExists()
    {
        // Arrange
        var customersResult = CustomerResponseModel.CustomerFakers(2);
        var customersFound = customersResult.ElementAt(1);
        var customerResult = CustomerFake.CustomerFaker();

        _customerService.Setup(x => x.GetById(customersFound.Id)).Returns(customerResult);

        // Act
        var customerResultFound = _customerAppService.GetById(customersResult.ElementAt(1).Id);

        // Assert
        customerResultFound.Should().NotBeNull();
        _customerService.Verify(x => x.GetById(customersFound.Id), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_CustomerDoesNotExists()
    {
        // Arrange
        var customerResult = CustomerFake.CustomerFaker();

        _customerService.Setup(x => x.GetById(customerResult.Id));

        // Act
        Action act = () => _customerAppService.GetById(customerResult.Id);

        // Assert
        act.Should().Throw<NotFoundException>($"Customer for Id: {customerResult.Id} was not found.");

        _customerService.Verify(x => x.GetById(customerResult.Id), Times.Once());
    }

    [Fact]
    public void Should_GetByName_When_CustomerExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFakers(2);
        var customer = customerFake.ElementAt(1);

        _customerService.Setup(x => x.GetAllByFullName(customer.FullName))
            .Returns((IList<Customer>)customerFake);

        // Act
        var customerFound = _customerAppService.GetByName(customer.FullName);

        // Assert
        customerFound.Should().NotBeNull();
        _customerService.Verify(x => x.GetAllByFullName(customer.FullName), Times.Once());
    }

    [Fact]
    public void Should_GetByName_When_CustomerDoesNotExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFakers(2);
        var customer = customerFake.ElementAt(1);

        _customerService.Setup(x => x.GetAllByFullName(customer.FullName));

        // Act
        Action act = () => _customerAppService.GetByName(customer.FullName);

        // Assert
        act.Should().Throw<NotFoundException>($"Customer for Id: {customer.FullName} was not found.");

        _customerService.Verify(x => x.GetAllByFullName(customer.FullName), Times.Once());
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
    public void Should_Delete_When_CustomerExists()
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
    public void Should_Delete_When_AccountBalanceGranThanZero()
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
            .Throw<BadRequestException>($"Customer needs to Withdraw the account balance before being deleted.");

        _customerBankInfoAppService.Verify(x => x.GetByCustomerId(bankInfoFake.CustomerId), Times.Once());
        _portfolioAppService.Verify(x => x.GetAllByCustomerId(bankInfoFake.CustomerId), Times.Once());
    }

    [Fact]
    public void Should_Delete_When_AnyTotalBalanceGranThanZero()
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
            .Throw<BadRequestException>($"Customer needs to withdraw the balance from the portfolio before being deleted.");

        _customerBankInfoAppService.Verify(x => x.GetByCustomerId(bankInfoFake.CustomerId), Times.Once());
        _portfolioAppService.Verify(x => x.GetAllByCustomerId(bankInfoFake.CustomerId), Times.Once());
    }
}
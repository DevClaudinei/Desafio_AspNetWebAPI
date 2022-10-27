using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Tests.EntitiesFake;
using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Moq;
using System.Linq.Expressions;

namespace DomainServices.Tests.Services;

public class CustomerServiceTests
{
    private readonly CustomerService _customerService;
    private readonly Mock<IUnitOfWork<ApplicationDbContext>> _mockUnitOfWork;
    private readonly Mock<IRepositoryFactory<ApplicationDbContext>> _mockRepositoryFactory;
    
    public CustomerServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork<ApplicationDbContext>>();
        _mockRepositoryFactory = new Mock<IRepositoryFactory<ApplicationDbContext>>();
        _customerService = new CustomerService(_mockUnitOfWork.Object, _mockRepositoryFactory.Object);
    }

    [Fact]
    public void Should_CreateCustomer_Sucessfully()
    {
        var customerFake = CustomerFake.CustomerFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Add(It.IsAny<Customer>()))
            .Returns(customerFake);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()));

        var customerId = _customerService.CreateCustomer(customerFake);
        customerId.Should().Be(customerFake.Id);

        _mockUnitOfWork.Verify(x => x.Repository<Customer>(), Times.Exactly(1));
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
        _mockUnitOfWork.Verify(x => x.Repository<Customer>().Add(customerFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_CreateCustomer_When_EmailAlreadyExists()
    {
        var customerFake = CustomerFake.CustomerFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>().Add(It.IsAny<Customer>()))
            .Throws<Exception>();
        _mockRepositoryFactory.SetupSequence(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(true)
            .Returns(false);

        Action act = () => _customerService.CreateCustomer(customerFake);
        act.Should().Throw<BadRequestException>($"Customer already exists for email: {customerFake.Email}");
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(1));
    }

    [Fact]
    public void Should_CreateCustomer_When_CpfAlreadyExists()
    {
        var customerFake = CustomerFake.CustomerFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>().Add(It.IsAny<Customer>()))
            .Throws<Exception>();
        _mockRepositoryFactory.SetupSequence(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(false)
            .Returns(true);

        Action act = () => _customerService.CreateCustomer(customerFake);
        act.Should().Throw<BadRequestException>($"Customer already exists for CPF: {customerFake.Cpf}");
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
    }

    [Fact]
    public void Should_GetAllCustomers_Sucessfully()
    {
        var customerFake = CustomerFake.CustomerFakers(5);
        var query = Mock.Of<IMultipleResultQuery<Customer>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Search(query)).Returns((IList<Customer>)customerFake);

        var customersFound = _customerService.GetAll();
        customersFound.Should().HaveCount(5);

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .MultipleResultQuery(), Times.Once());
    }

    [Fact]
    public void Should_GetAllCustomers_When_ThereIsNoCustomer()
    {
        var query = Mock.Of<IMultipleResultQuery<Customer>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .MultipleResultQuery()).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Search(query)).Returns(new List<Customer>());

        var customersFound = _customerService.GetAll();
        
        customersFound.Should().BeEmpty();
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .MultipleResultQuery(), Times.Once());
    }

    [Fact]
    public void Should_GetCustomerById_When_CustomerExists()
    {
        var customerFake = CustomerFake.CustomerFaker();
        var query = Mock.Of<IQuery<Customer>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(query)).Returns(customerFake);

        var customerFound = _customerService.GetById(customerFake.Id);
        customerFound.Id.Should().Be(1);

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleOrDefault(query), Times.Once());
    }

    [Fact]
    public void Should_GetCustomerById_When_CustomerDoesNotExists()
    {
        var customerFake = CustomerFake.CustomerFaker();
        var query = Mock.Of<IQuery<Customer>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(query));

        var customerFound = _customerService.GetById(customerFake.Id);
        customerFound.Should().BeNull();
    }

    [Fact]
    public void Should_GetAllCustomerByFullNameExists()
    {
        var customerFake = CustomerFake.CustomerFakers(5);
        var query = Mock.Of<IQuery<Customer>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Search(query)).Returns((IList<Customer>)customerFake);

        var customerFound = _customerService.GetAllByFullName(customerFake.ElementAt(1).FullName);
        customerFound.Should().HaveCountGreaterThanOrEqualTo(1);

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Search(query), Times.Once());
    }

    [Fact]
    public void Should_GetAllCustomerByFullNameDoesNotExists()
    {
        var customerFake = CustomerFake.CustomerFakers(5);
        var query = Mock.Of<IMultipleResultQuery<Customer>>();

        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Search(query)).Returns(new List<Customer>());

        var customersFound = _customerService.GetAllByFullName(customerFake.ElementAt(1).FullName);
        customersFound.Should().BeEmpty();
    }

    [Fact]
    public void Should_UpdateCustmer_When_CustomerExists()
    {
        var customerFake = CustomerFake.CustomerFaker();
        var query = Mock.Of<IQuery<Customer>>();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(query)).Returns(customerFake);

        _customerService.Update(customerFake.Id, customerFake);

        _mockUnitOfWork.Verify(x => x.Repository<Customer>(), Times.Exactly(1));
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
        _mockUnitOfWork.Verify(x => x.Repository<Customer>().Update(customerFake), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_UpdateCustmer_When_CustomerDoesNotExists()
    {
        var customerFake = CustomerFake.CustomerFaker();
        var query = Mock.Of<IQuery<Customer>>();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(query));

        Action act = () => _customerService.Update(customerFake.Id, customerFake);
        act.Should().Throw<NotFoundException>($"Client for Id: {customerFake.Id} not found.");
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
    }

    [Fact]
    public void Should_UpdateCustomer_When_CpfAlreadyExists()
    {
        var customerFake = CustomerFake.CustomerFaker();
        var query = Mock.Of<IQuery<Customer>>();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()));
        _mockRepositoryFactory.SetupSequence(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(false)
            .Returns(true);

        Action act = () => _customerService.Update(customerFake.Id, customerFake);
        act.Should().Throw<BadRequestException>($"Customer already exists for CPF: {customerFake.Cpf}");
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
    }

    [Fact]
    public void Should_UpdateCustomer_When_EmailAlreadyExists()
    {
        var customerFake = CustomerFake.CustomerFaker();
        var query = Mock.Of<IQuery<Customer>>();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()));
        _mockRepositoryFactory.SetupSequence(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(true)
            .Returns(false);

        Action act = () => _customerService.Update(customerFake.Id, customerFake);
        act.Should().Throw<BadRequestException>($"Customer already exists for email: {customerFake.Email}");
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(1));
    }

    [Fact]
    public void Should_DeleteCustomer_When_CustomerExists()
    {
        var customerFake = CustomerFake.CustomerFaker();
        var query = Mock.Of<IQuery<Customer>>();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Remove(It.IsAny<Customer>()));
        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Remove(It.IsAny<Expression<Func<Customer, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(query)).Returns(customerFake);

        _customerService.Delete(customerFake.Id);
        _mockUnitOfWork.Verify(x => x.Repository<Customer>()
            .Remove(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_DeleteCustomer_When_CustomerDoesNotExists()
    {
        var customerFake = CustomerFake.CustomerFaker();
        var query = Mock.Of<IQuery<Customer>>();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(query);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(query));

        Action act = () => _customerService.Delete(customerFake.Id);
        act.Should().Throw<NotFoundException>($"Client for Id: {customerFake.Id} not found.");
    }
}
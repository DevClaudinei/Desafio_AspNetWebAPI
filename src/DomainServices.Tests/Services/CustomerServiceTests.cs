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
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Add(It.IsAny<Customer>()))
            .Returns(customerFake);
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()));

        // Act
        var customerId = _customerService.CreateCustomer(customerFake);

        // Assert
        customerId.Should().Be(customerFake.Id);

        _mockUnitOfWork.Verify(x => x.Repository<Customer>().Add(customerFake), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_CreateCustomer_When_EmailAlreadyExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>().Add(It.IsAny<Customer>()));
        _mockRepositoryFactory.SetupSequence(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(true)
            .Returns(false);

        // Act
        Action act = () => _customerService.CreateCustomer(customerFake);

        // Assert
        act.Should().Throw<BadRequestException>($"Customer already exists for email: {customerFake.Email}");

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(1));
        _mockUnitOfWork.Verify(x => x.Repository<Customer>().Add(It.IsAny<Customer>()), Times.Never());
    }

    [Fact]
    public void Should_CreateCustomer_When_CpfAlreadyExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();

        _mockUnitOfWork.Setup(x => x.Repository<Customer>().Add(It.IsAny<Customer>()));
        _mockRepositoryFactory.SetupSequence(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(false)
            .Returns(true);

        // Act
        Action act = () => _customerService.CreateCustomer(customerFake);

        // Assert
        act.Should().Throw<BadRequestException>($"Customer already exists for CPF: {customerFake.Cpf}");

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
        _mockUnitOfWork.Verify(x => x.Repository<Customer>().Add(It.IsAny<Customer>()), Times.Never());
    }

    [Fact]
    public void Should_GetAll_When_CustomersExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFakers(5);
        
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Search(It.IsAny<IMultipleResultQuery<Customer>>()))
            .Returns((IList<Customer>)customerFake);

        // Act
        var customersFound = _customerService.GetAll();

        // Assert
        customersFound.Should().HaveCount(5);

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Search(It.IsAny<IMultipleResultQuery<Customer>>()), Times.Once());
    }

    [Fact]
    public void Should_GetAll_When_CustomersDoesNotExists()
    {
        // Arrange
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .MultipleResultQuery())
            .Returns(It.IsAny<IMultipleResultQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Search(It.IsAny<IMultipleResultQuery<Customer>>()))
            .Returns(new List<Customer>());

        // Act
        var customersFound = _customerService.GetAll();
        
        // Assert
        customersFound.Should().BeEmpty();

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .MultipleResultQuery(), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Search(It.IsAny<IMultipleResultQuery<Customer>>()), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_CustomerExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(It.IsAny<IQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()))
            .Returns(customerFake);

        // Act
        var customerFound = _customerService.GetById(customerFake.Id);

        // Assert
        customerFound.Id.Should().Be(customerFake.Id);

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()), Times.Once());
    }

    [Fact]
    public void Should_GetById_When_CustomerDoesNotExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(It.IsAny<IQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()));

        // Act
        var customerFound = _customerService.GetById(customerFake.Id);

        // Assert
        customerFound.Should().BeNull();

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()), Times.Once());
    }

    [Fact]
    public void Should_GetAllByFullName_When_CustomersExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFakers(5);
        
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(It.IsAny<IQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Search(It.IsAny<IQuery<Customer>>()))
            .Returns((IList<Customer>)customerFake);

        // Act
        var customerFound = _customerService.GetAllByFullName(customerFake.ElementAt(1).FullName);

        // Assert
        customerFound.Should().HaveCountGreaterThanOrEqualTo(1);

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Search(It.IsAny<IQuery<Customer>>()), Times.Once());
    }

    [Fact]
    public void Should_GetAllByFullName_When_CustomersDoesNotExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFakers(5);
        
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .MultipleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(It.IsAny<IQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Search(It.IsAny<IQuery<Customer>>())).Returns(new List<Customer>());

        // Act
        var customersFound = _customerService.GetAllByFullName(customerFake.ElementAt(1).FullName);

        // Assert
        customersFound.Should().BeEmpty();

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
           .MultipleResultQuery()
           .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Search(It.IsAny<IQuery<Customer>>()), Times.Once());
    }

    [Fact]
    public void Should_Update_When_CustomerExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();
        
        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(It.IsAny<IQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>())).Returns(customerFake);

        // Act
        _customerService.Update(customerFake.Id, customerFake);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<Customer>()
           .Update(It.IsAny<Customer>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
    }

    [Fact]
    public void Should_Update_When_CustomerDoesNotExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();
        
        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()));
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(It.IsAny<IQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()));

        // Act
        Action act = () => _customerService.Update(customerFake.Id, customerFake);

        // Assert
        act.Should().Throw<NotFoundException>($"Client for Id: {customerFake.Id} not found.");

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()), Times.Exactly(1));
        _mockUnitOfWork.Verify(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()), Times.Never());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Never());
    }

    [Fact]
    public void Should_Update_When_CpfAlreadyExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();
        
        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()));
        _mockRepositoryFactory.SetupSequence(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(false)
            .Returns(true);

        // Act
        Action act = () => _customerService.Update(customerFake.Id, customerFake);

        // Assert
        act.Should().Throw<BadRequestException>($"Customer already exists for CPF: {customerFake.Cpf}");

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(2));
        _mockUnitOfWork.Verify(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()), Times.Never());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Never());
    }

    [Fact]
    public void Should_Update_When_EmailAlreadyExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();
        
        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()));
        _mockRepositoryFactory.SetupSequence(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(true)
            .Returns(false);

        // Act
        Action act = () => _customerService.Update(customerFake.Id, customerFake);

        // Assert
        act.Should().Throw<BadRequestException>($"Customer already exists for email: {customerFake.Email}");

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .Any(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Exactly(1));
        _mockUnitOfWork.Verify(x => x.Repository<Customer>()
            .Update(It.IsAny<Customer>()), Times.Never());
        _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Never());
    }

    [Fact]
    public void Should_Delete_When_CustomerExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(It.IsAny<IQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()))
        .Returns(customerFake);
        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Remove(It.IsAny<Expression<Func<Customer, bool>>>()));

        // Act
        _customerService.Delete(customerFake.Id);

        // Assert
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<Customer>()
            .Remove(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
    }

    [Fact]
    public void Should_Delete_When_CustomerDoesNotExists()
    {
        // Arrange
        var customerFake = CustomerFake.CustomerFaker();
        
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()))
            .Returns(It.IsAny<IQuery<Customer>>());
        _mockRepositoryFactory.Setup(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()));
        _mockUnitOfWork.Setup(x => x.Repository<Customer>()
            .Remove(It.IsAny<Customer>()));

        // Act
        Action act = () => _customerService.Delete(customerFake.Id);

        // Assert
        act.Should().Throw<NotFoundException>($"Client for Id: {customerFake.Id} not found.");

        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleResultQuery()
            .AndFilter(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once());
        _mockRepositoryFactory.Verify(x => x.Repository<Customer>()
            .SingleOrDefault(It.IsAny<IQuery<Customer>>()), Times.Once());
        _mockUnitOfWork.Verify(x => x.Repository<Customer>()
            .Remove(It.IsAny<Customer>()), Times.Never());
    }
}
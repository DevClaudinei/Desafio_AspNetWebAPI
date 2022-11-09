using Application.Models.Customer.Requests;
using AppServices.Tests.ModelsFake.Customer;
using AppServices.Validations.Customer;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;

namespace AppServices.Tests.Validations.Customer;

public class CustomerCreateValidatorTests
{
    [Fact]
    public void Should_Verify_Customer_Is_Valid_To_Create()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        var validator = new CustomerCreateValidator();

        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_Verify_Customer_When_FullName_Is_Empty()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: "", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();
        
        var validator = new CustomerCreateValidator();
        
        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(4);
    }

    [Fact]
    public void Should_Verify_Customer_When_FullName_Does_Not_Have_LastName()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: "José", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();

        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Verify_Customer_When_FullName_Contais_Invalid_Characters()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: "Jo@o Conceição", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();
        
        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Verify_Customer_When_FullName_Contains_Unnecessary_White_Spaces()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: "Solange  Braga", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();

        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Should_Verify_Customer_When_LastName_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: "Maria A Santos", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();
        
        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Verify_Customer_When_Email_Is_Empty()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();
        
        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
    }

    [Fact]
    public void Should_Verify_Customer_When_Email_Is_Different_From_Emailconfirmation()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "clro@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();

        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Verify_Customer_When_Cpf_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();

        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Verify_Customer_When_Cellphone_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11)987451892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();

        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Verify_Customer_When_Has_Not_Reached_Adult_hood()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: DateTime.UtcNow.AddYears(-18),
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();

        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Verify_Customer_When_PostalCode_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: "48280000", address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerCreateValidator();

        // Act
        var result = validator.Validate(customerFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }
}
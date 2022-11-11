using Application.Models.Customer.Requests;
using AppServices.Tests.ModelsFake.Customer;
using AppServices.Validations.Customer;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace AppServices.Tests.Validations.Customer;

public class CustomerUpdateValidatorTests
{
    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_FullName_Is_Empty()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: "", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_FullName_Does_Not_Have_LastName()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: "José", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_FullName_Contais_Invalid_Characters()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: "Jo@o Conceição", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_FullName_Contains_Unnecessary_White_Spaces()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: "Solange  Braga", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_LastName_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: "Maria A Santos", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_Email_Is_Empty()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: f.Person.FullName, email: "", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_Email_Is_Different_From_Emailconfirmation()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: f.Person.FullName, email: "clro@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.Equals(x.EmailConfirmation));
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_Cpf_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_Cellphone_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11)987451892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CellPhone);
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_Has_Not_Reached_Adult_hood()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: DateTime.UtcNow.AddYears(-18),
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Should_Pass_When_Run_CustomerUpdateValidator_Because_PostalCode_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(false), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: "48280000", address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }
}
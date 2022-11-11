using Application.Models.Customer.Requests;
using AppServices.Tests.ModelsFake.Customer;
using AppServices.Validations.Customer;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;

namespace AppServices.Tests.Validations.Customer;

public class StringExtensionsTests
{
    [Fact]
    public void Should_Pass_When_Run_IsValidDocument_When_Document_Has_No_Mask()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();

        // Act
        var result = StringExtensions.IsValidDocument(customerFake.Cpf);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Run_IsValidDocument_Because_Document_Has_Mask()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.IsValidDocument(customerFake.Cpf);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Run_IsValidDocument_Because_Cellphone_Has_Format_Correct()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(), cellphone: "(11) 98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.IsCellphone(customerFake.Cellphone);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Run_IsCellphone_Because_Cellphone_Has_Format_Incorrect()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.IsCellphone(customerFake.Cellphone);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Run_IsPostalCode_Because_PostalCode_Has_Format_Correct()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
                cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
                city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.IsPostalCode(customerFake.PostalCode);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Run_IsPostalCode_Because_PostalCode_Has_Format_Incorrect()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
        fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
        cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: f.Person.DateOfBirth,
        emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
        city: f.Address.City(), postalCode: "41.250-540", address: f.Address.StreetAddress(),
        number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.IsPostalCode(customerFake.PostalCode);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Run_HasReachedAdulthood_Because_Customer_Has_Reached_AdultHood()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
        fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
        cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: f.Person.DateOfBirth,
        emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
        city: f.Address.City(), postalCode: "41.250-540", address: f.Address.StreetAddress(),
        number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.HasReachedAdulthood(customerFake.DateOfBirth);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Run_HasReachedAdulthood_Because_Has_Not_Reached_AdultHood()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
        fullName: f.Person.FullName, email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
        cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: new DateTime(2005, 11, 8, 18, 0, 0),
        emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
        city: f.Address.City(), postalCode: "41.250-540", address: f.Address.StreetAddress(),
        number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.HasReachedAdulthood(customerFake.DateOfBirth);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Run_ContainsEmptySpace_Because_FullName_Does_Not_Contains_Empty_Space()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
        fullName: "Andrea  Santos", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
        cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: new DateTime(2005, 11, 8, 18, 0, 0),
        emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
        city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
        number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.ContainsEmptySpace(customerFake.FullName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Run_ContainsEmptySpace_Because_FullName_Contains_Empty_Space()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
        fullName: "Gabriela Fux", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
        cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: new DateTime(2005, 11, 8, 18, 0, 0),
        emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
        city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
        number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.ContainsEmptySpace(customerFake.FullName);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Run_AnySymbolOrSpecialCharacter_Because_FullName_Contains_Any_Symbol_Or_Special_Character()
    {
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
        fullName: "Jo@na Alcantara", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
        cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: new DateTime(2005, 11, 8, 18, 0, 0),
        emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
        city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
        number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.AnySymbolOrSpecialCharacter(customerFake.FullName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Run_AnySymbolOrSpecialCharacter_Because_FullName_Does_Not_Contains_Any_Symbol_Or_Special_Character()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
        fullName: "Joana Alcantara", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
        cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: new DateTime(2005, 11, 8, 18, 0, 0),
        emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
        city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
        number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.AnySymbolOrSpecialCharacter(customerFake.FullName);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Run_HasAtLeastTwoCharactersForEachWord_Because_FullName_HasAtLeastTwoCharactersForEachWord()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
        fullName: "Josafa Andrade", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
        cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: new DateTime(2005, 11, 8, 18, 0, 0),
        emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
        city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
        number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.HasAtLeastTwoCharactersForEachWord(customerFake.FullName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Run_HasAtLeastTwoCharactersForEachWord_Because_FullName_HasNotAtLeastTwoCharactersForEachWord()
    {
        // Arrange
        var customerFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
        fullName: "Josafa A", email: "clr@gpx.com", emailConfirmation: "clr@gpx.com",
        cpf: f.Person.Cpf(), cellphone: "(11)98745-1892", dateOfBirth: new DateTime(2005, 11, 8, 18, 0, 0),
        emailSms: f.Random.Bool(), whatsApp: f.Random.Bool(), country: f.Address.Country(),
        city: f.Address.City(), postalCode: f.Address.ZipCode(), address: f.Address.StreetAddress(),
        number: f.Random.Number(1, 10000))).Generate();

        // Act
        var result = StringExtensions.HasAtLeastTwoCharactersForEachWord(customerFake.FullName);

        // Assert
        result.Should().BeFalse();
    }
}
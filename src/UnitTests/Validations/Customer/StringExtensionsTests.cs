using AppServices.Validations.Customer;
using FluentAssertions;
using System;
using UnitTests.EntitiesFake.Customers;

namespace UnitTests.Validations.Customer;

public class StringExtensionsTests
{
    [Fact]
    public void Should_Pass_When_Executing_IsValidDocument_When_Document_Has_No_Mask()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();

        // Act
        var result = customerFake.Cpf.IsValidDocument();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_IsValidDocument_Because_Document_Has_Mask()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.Cpf = "008.216.795-89";

        // Act
        var result = customerFake.Cpf.IsValidDocument();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Executing_IsValidDocument_Because_Cellphone_Has_Format_Correct()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.Cellphone = "(11) 99876-3635";

        // Act
        var result = customerFake.Cellphone.IsCellphone();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_IsCellphone_Because_Cellphone_Has_Format_Incorrect()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.Cellphone = "(11)99876-3635";

        // Act
        var result = customerFake.Cellphone.IsCellphone();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Executing_IsPostalCode_Because_PostalCode_Has_Format_Correct()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.PostalCode = "48280-000";

        // Act
        var result = customerFake.PostalCode.IsPostalCode();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_IsPostalCode_Because_PostalCode_Has_Format_Incorrect()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.PostalCode = "48280000";

        // Act
        var result = customerFake.PostalCode.IsPostalCode();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Executing_HasReachedAdulthood_Because_Customer_Has_Reached_AdultHood()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.DateOfBirth = new DateTime(1977, 11, 8, 18, 0, 0);

        // Act
        var result = customerFake.DateOfBirth.HasReachedAdulthood();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_HasReachedAdulthood_Because_Has_Not_Reached_AdultHood()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.DateOfBirth = new DateTime(2005, 11, 8, 18, 0, 0);

        // Act
        var result = customerFake.DateOfBirth.HasReachedAdulthood();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Executing_ContainsEmptySpace_Because_FullName_Does_Not_Contains_Empty_Space()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "Andrea Santos";

        // Act
        var result = customerFake.FullName.ContainsEmptySpace();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_When_Executing_ContainsEmptySpace_Because_FullName_Contains_Empty_Space()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "Gabriela  Fux";

        // Act
        var result = customerFake.FullName.ContainsEmptySpace();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Pass_When_Executing_AnySymbolOrSpecialCharacter_Because_FullName_Contains_Any_Symbol_Or_Special_Character()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "Joana Alcantara";

        // Act
        var result = customerFake.FullName.AnySymbolOrSpecialCharacter();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_When_Executing_AnySymbolOrSpecialCharacter_Because_FullName_Does_Not_Contains_Any_Symbol_Or_Special_Character()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "Jo@na Alcantara";

        // Act
        var result = customerFake.FullName.AnySymbolOrSpecialCharacter();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Pass_When_Executing_HasAtLeastTwoCharactersForEachWord_Because_FullName_HasAtLeastTwoCharactersForEachWord()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "Josafa Andrade";

        // Act
        var result = customerFake.FullName.HasAtLeastTwoCharactersForEachWord();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_HasAtLeastTwoCharactersForEachWord_Because_FullName_HasAtLeastTwoCharactersForEachWord()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "Josafa A";

        // Act
        var result = customerFake.FullName.HasAtLeastTwoCharactersForEachWord();

        // Assert
        result.Should().BeFalse();
    }
}
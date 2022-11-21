using AppServices.Validations.Customer;
using FluentAssertions;
using System;

namespace UnitTests.Validations.Customer;

public class StringExtensionsTests
{
    [Fact]
    public void Should_Pass_When_Executing_IsValidDocument_When_Document_Has_No_Mask()
    {
        // Arrange
        var cpf = "00821679589";

        // Act
        var result = cpf.IsValidDocument();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_IsValidDocument_Because_Document_Has_Mask()
    {
        // Arrange
        var cpf = "008.216.795-89";

        // Act
        var result = cpf.IsValidDocument();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Executing_IsValidDocument_Because_Cellphone_Has_Format_Correct()
    {
        // Arrange
        var cellphone = "(11) 99876-3635";

        // Act
        var result = cellphone.IsCellphone();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_IsCellphone_Because_Cellphone_Has_Format_Incorrect()
    {
        // Arrange
        var cellphone = "(11)99876-3635";

        // Act
        var result = cellphone.IsCellphone();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Executing_IsPostalCode_Because_PostalCode_Has_Format_Correct()
    {
        // Arrange
        var postalCode = "48280-000";

        // Act
        var result = postalCode.IsPostalCode();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_IsPostalCode_Because_PostalCode_Has_Format_Incorrect()
    {
        // Arrange
        var postalCode = "48.280-000";

        // Act
        var result = postalCode.IsPostalCode();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Executing_HasReachedAdulthood_Because_Customer_Has_Reached_AdultHood()
    {
        // Arrange
        var dateOfBirth = new DateTime(1977, 11, 8, 18, 0, 0);

        // Act
        var result = dateOfBirth.HasReachedAdulthood();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_HasReachedAdulthood_Because_Has_Not_Reached_AdultHood()
    {
        // Arrange
        var dateOfBirth = new DateTime(2005, 11, 8, 18, 0, 0);

        // Act
        var result = dateOfBirth.HasReachedAdulthood();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Pass_When_Executing_ContainsEmptySpace_Because_FullName_Does_Not_Contains_Empty_Space()
    {
        // Arrange
        var fullName = "Andrea Santos";

        // Act
        var result = fullName.ContainsEmptySpace();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_When_Executing_ContainsEmptySpace_Because_FullName_Contains_Empty_Space()
    {
        // Arrange
        var fullName = "Gabriela  Fux";

        // Act
        var result = fullName.ContainsEmptySpace();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Pass_When_Executing_AnySymbolOrSpecialCharacter_Because_FullName_Contains_Any_Symbol_Or_Special_Character()
    {
        // Arrange
        var fullName = "Joana Alcantara";

        // Act
        var result = fullName.AnySymbolOrSpecialCharacter();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_When_Executing_AnySymbolOrSpecialCharacter_Because_FullName_Does_Not_Contains_Any_Symbol_Or_Special_Character()
    {
        // Arrange
        var fullName = "Jo@na Alcantara";

        // Act
        var result = fullName.AnySymbolOrSpecialCharacter();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Pass_When_Executing_HasAtLeastTwoCharactersForEachWord_Because_FullName_HasAtLeastTwoCharactersForEachWord()
    {
        // Arrange
        var fullName = "Josafa Andrade";

        // Act
        var result = fullName.HasAtLeastTwoCharactersForEachWord();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_When_Executing_HasAtLeastTwoCharactersForEachWord_Because_FullName_HasAtLeastTwoCharactersForEachWord()
    {
        // Arrange
        var fullName = "Josafa A";

        // Act
        var result = fullName.HasAtLeastTwoCharactersForEachWord();

        // Assert
        result.Should().BeFalse();
    }
}
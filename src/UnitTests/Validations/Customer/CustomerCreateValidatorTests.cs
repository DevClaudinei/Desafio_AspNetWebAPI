using AppServices.Validations.Customer;
using FluentValidation.TestHelper;
using System;
using UnitTests.EntitiesFake.Customers;

namespace UnitTests.Validations.Customer;

public class CustomerCreateValidatorTests
{
    private readonly CustomerCreateValidator _customerCreateValidator = new CustomerCreateValidator();

    [Fact]
    public void Should_Pass_When_Executing_CustomerCreateValidator()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        
        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_FullName_Is_Empty()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_FullName_Does_Not_Have_LastName()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "José";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_FullName_Contais_Invalid_Characters()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "Jo@o Conceição";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_FullName_Contains_Unnecessary_White_Spaces()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "Solange  Braga";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_LastName_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.FullName = "Maria A Santos";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_Email_Is_Empty()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.Email = "";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_Email_Is_Different_From_Emailconfirmation()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.EmailConfirmation = "clr@gpx.com";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_Cpf_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.Cpf = "008.216.795-89";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_Cellphone_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.Cellphone = "(11)995479812";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cellphone);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_Has_Not_Reached_Adult_hood()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.DateOfBirth = DateTime.UtcNow.AddYears(-18);

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Should_Fail_When_Executing_CustomerCreateValidator_Because_PostalCode_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = CreateCustomerModel.CustomerFaker();
        customerFake.PostalCode = "48280000";

        // Act
        var result = _customerCreateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }
}
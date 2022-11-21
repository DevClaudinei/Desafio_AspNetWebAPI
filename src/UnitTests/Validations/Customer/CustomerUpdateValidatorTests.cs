using AppServices.Validations.Customer;
using FluentValidation.TestHelper;
using System;
using UnitTests.EntitiesFake.Customers;

namespace UnitTests.Validations.Customer;

public class CustomerUpdateValidatorTests
{
    private readonly CustomerUpdateValidator _customerUpdateValidator = new CustomerUpdateValidator();
        
    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        
        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_FullName_Is_Empty()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_FullName_Does_Not_Have_LastName()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "José";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_FullName_Contais_Invalid_Characters()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "Jo@o Conceição";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_FullName_Contains_Unnecessary_White_Spaces()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "Solange  Braga";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_LastName_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "Maria A Santos";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Email_Is_Empty()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.Email = "";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Email_Is_Different_From_Emailconfirmation()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.Email = "clro@gpx.com";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.Equals(x.EmailConfirmation));
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Cpf_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.Cpf = "008.216.795-89";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Cellphone_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.CellPhone = "(11) 975681439";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CellPhone);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Has_Not_Reached_Adult_hood()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.DateOfBirth = DateTime.UtcNow.AddYears(-18);

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_PostalCode_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.PostalCode = "48280000";

        // Act
        var result = _customerUpdateValidator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }
}
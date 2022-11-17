using AppServices.Validations.Customer;
using FluentValidation.TestHelper;
using System;
using UnitTests.EntitiesFake.Customers;

namespace UnitTests.Validations.Customer;

public class CustomerUpdateValidatorTests
{
    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator()
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
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_FullName_Is_Empty()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_FullName_Does_Not_Have_LastName()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "José";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_FullName_Contais_Invalid_Characters()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "Jo@o Conceição";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_FullName_Contains_Unnecessary_White_Spaces()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "Solange  Braga";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_LastName_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.FullName = "Maria A Santos";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Email_Is_Empty()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.Email = "";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Email_Is_Different_From_Emailconfirmation()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.Email = "clro@gpx.com";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email.Equals(x.EmailConfirmation));
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Cpf_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.Cpf = "008.216.795-89";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Cellphone_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.CellPhone = "(11) 975681439";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CellPhone);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_Has_Not_Reached_Adult_hood()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.DateOfBirth = DateTime.UtcNow.AddYears(-18);

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Should_Pass_When_Executing_CustomerUpdateValidator_Because_PostalCode_Format_Is_Invalid()
    {
        // Arrange
        var customerFake = UpdateCustomerModel.CustomerFaker();
        customerFake.PostalCode = "48280000";

        var validator = new CustomerUpdateValidator();

        // Act
        var result = validator.TestValidate(customerFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PostalCode);
    }
}
using AppServices.Validations.Product;
using FluentValidation.TestHelper;
using UnitTests.EntitiesFake.Products;

namespace UnitTests.Validations.Product;

public class ProductUpdateValidatorTests
{
    [Fact]
    public void Should_Pass_When_Executing_ProductUpdateValidator_Because_Product_Is_Valid()
    {
        // Arrange
        var productFake = UpdateProductModel.ProductFake();
        var validator = new ProductUpdateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Executing_ProductUpdateValidator_Because_Symbol_Is_Empty()
    {
        // Arrange
        var productFake = UpdateProductModel.ProductFake();
        productFake.Symbol = "";

        var validator = new ProductUpdateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

    [Fact]
    public void Should_Fail_When_Executing_ProductUpdateValidator_Because_Symbol_Is_Null()
    {
        // Arrange
        var productFake = UpdateProductModel.ProductFake();
        productFake.Symbol = null;

        var validator = new ProductUpdateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

    [Fact]
    public void Should_Pass_When_Executing_ProductUpdateValidator_Because_UnitPrice_Is_Zero()
    {
        // Arrange
        var productFake = UpdateProductModel.ProductFake();
        productFake.UnitPrice = 0;

        var validator = new ProductUpdateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UnitPrice);
    }
}
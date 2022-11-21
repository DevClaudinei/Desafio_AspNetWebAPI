using AppServices.Validations.Product;
using FluentValidation.TestHelper;
using UnitTests.EntitiesFake.Products;

namespace UnitTests.Validations.Product;

public class ProductCreateValidatorTests
{
    private readonly ProductCreateValidator _productCreateValidator = new ProductCreateValidator();

    [Fact]
    public void Should_Pass_When_Executing_ProductCreateValidator_Because_Product_Is_Valid()
    {
        // Arrange
        var productFake = CreateProductModel.ProductFake();
        
        // Act
        var result = _productCreateValidator.TestValidate(productFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Executing_ProductCreateValidator_Because_Symbol_Is_Empty()
    {
        // Arrange
        var productFake = CreateProductModel.ProductFake();
        productFake.Symbol = "";

        // Act
        var result = _productCreateValidator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

    [Fact]
    public void Should_Fail_When_Executing_ProductCreateValidator_Because_Symbol_Is_Null()
    {
        // Arrange
        var productFake = CreateProductModel.ProductFake();
        productFake.Symbol = null;

        // Act
        var result = _productCreateValidator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

    [Fact]
    public void Should_Fail_When_Executing_ProductCreateValidator_Because_UnitPrice_Is_Zero()
    {
        // Arrange
        var productFake = CreateProductModel.ProductFake();
        productFake.UnitPrice = 0;

        // Act
        var result = _productCreateValidator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UnitPrice);
    }
}
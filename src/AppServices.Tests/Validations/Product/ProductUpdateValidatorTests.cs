using Application.Models.Product.Request;
using AppServices.Tests.ModelsFake.Product;
using AppServices.Validations.Product;
using Bogus;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace AppServices.Tests.Validations.Product;

public class ProductUpdateValidatorTests
{
    [Fact]
    public void Should_Pass_When_Run_ProductUpdateValidator_Because_Product_Is_Valid()
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
    public void Should_Fail_When_Run_ProductUpdateValidator_Because_Symbol_Is_Empty()
    {
        // Arrange
        var productFake = new Faker<UpdateProductRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateProductRequest(
                symbol: "",
                unitPrice: f.Random.Decimal(1, 100)
            )).Generate();

        var validator = new ProductUpdateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

    [Fact]
    public void Should_Fail_When_Run_ProductUpdateValidator_Because_Symbol_Is_Null()
    {
        // Arrange
        var productFake = new Faker<UpdateProductRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateProductRequest(
                symbol: null,
                unitPrice: f.Random.Decimal(1, 100)
            )).Generate();

        var validator = new ProductUpdateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

    [Fact]
    public void Should_Pass_When_Run_ProductUpdateValidator_Because_UnitPrice_Is_Empty()
    {
        // Arrange
        var productFake = new Faker<UpdateProductRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateProductRequest(
                symbol: f.Commerce.ProductName(),
                unitPrice: 0
            )).Generate();

        var validator = new ProductUpdateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UnitPrice);
    }
}
using Application.Models.Product.Request;
using AppServices.Tests.ModelsFake.Product;
using AppServices.Validations.Product;
using Bogus;
using FluentValidation.TestHelper;

namespace AppServices.Tests.Validations.Product;

public class ProductCreateValidatorTests
{
    [Fact]
    public void Should_Pass_When_Run_ProductCreateValidator_Because_Product_Is_Valid()
    {
        // Arrange
        var productFake = CreateProductModel.ProductFake();
        var validator = new ProductCreateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Run_ProductCreateValidator_Because_Symbol_Is_Empty()
    {
        // Arrange
        var productFake = new Faker<CreateProductRequest>("pt_BR")
            .CustomInstantiator(f => new CreateProductRequest(
                symbol: "",
                unitPrice: f.Random.Decimal(1, 100)
            )).Generate();

        var validator = new ProductCreateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

    [Fact]
    public void Should_Fail_When_Run_ProductCreateValidator_Because_Symbol_Is_Null()
    {
        // Arrange
        var productFake = new Faker<CreateProductRequest>("pt_BR")
            .CustomInstantiator(f => new CreateProductRequest(
                symbol: null,
                unitPrice: f.Random.Decimal(1, 100)
            )).Generate();

        var validator = new ProductCreateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Symbol);
    }

    [Fact]
    public void Should_Fail_When_Run_ProductCreateValidator_Because_UnitPrice_Is_Empty()
    {
        // Arrange
        var productFake = new Faker<CreateProductRequest>("pt_BR")
            .CustomInstantiator(f => new CreateProductRequest(
                symbol: f.Commerce.ProductName(),
                unitPrice: 0
            )).Generate();
        
        var validator = new ProductCreateValidator();

        // Act
        var result = validator.TestValidate(productFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UnitPrice);
    }
}
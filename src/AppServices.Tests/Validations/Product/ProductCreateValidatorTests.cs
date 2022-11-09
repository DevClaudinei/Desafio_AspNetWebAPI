using Application.Models.Product.Request;
using AppServices.Tests.ModelsFake.Product;
using AppServices.Validations.Product;
using Bogus;
using FluentAssertions;

namespace AppServices.Tests.Validations.Product;

public class ProductCreateValidatorTests
{
    [Fact]
    public void Should_Verify_Create_When_Product_Is_Valid()
    {
        // Arrange
        var productFake = CreateProductModel.ProductFake();
        var validator = new ProductCreateValidator();

        // Act
        var result = validator.Validate(productFake);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_Verify_Create_When_Symbol_Is_Empty()
    {
        // Arrange
        var productFake = new Faker<CreateProductRequest>("pt_BR")
            .CustomInstantiator(f => new CreateProductRequest(
                symbol: "",
                unitPrice: f.Random.Decimal(1, 100)
            )).Generate();

        var validator = new ProductCreateValidator();

        // Act
        var result = validator.Validate(productFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Verify_Create_When_Symbol_Is_Null()
    {
        // Arrange
        var productFake = new Faker<CreateProductRequest>("pt_BR")
            .CustomInstantiator(f => new CreateProductRequest(
                symbol: null,
                unitPrice: f.Random.Decimal(1, 100)
            )).Generate();

        var validator = new ProductCreateValidator();

        // Act
        var result = validator.Validate(productFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Should_Verify_Create_When_UnitPrice_Is_Empty()
    {
        // Arrange
        var productFake = new Faker<CreateProductRequest>("pt_BR")
            .CustomInstantiator(f => new CreateProductRequest(
                symbol: f.Commerce.ProductName(),
                unitPrice: 0
            )).Generate();

        var validator = new ProductCreateValidator();

        // Act
        var result = validator.Validate(productFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }
}
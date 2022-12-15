using AppServices.Validations.Portfolio;
using FluentValidation.TestHelper;
using UnitTests.EntitiesFake.Portfolios;

namespace UnitTests.Validations.Portfolio;

public class UninvestmentCreateValidatorTests
{
    private readonly UninvestmentCreateValidator _uninvestmentCreateValidator = new UninvestmentCreateValidator();

    [Fact]
    public void Should_Pass_When_Executing_InvestmentCreateValidator()
    {
        // Arrange
        var investmentFake = CreateUninvestmentModel.UninvestmentFake();
        
        // Act
        var result = _uninvestmentCreateValidator.TestValidate(investmentFake);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ConvertedAt);
    }

    [Fact]
    public void Should_Fail_When_Executing_InvestmentCreateValidator()
    {
        // Arrange
        var investmentFake = CreateUninvestmentModel.UninvestmentInvalid();
        
        // Act
        var result = _uninvestmentCreateValidator.TestValidate(investmentFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConvertedAt);
    }
}
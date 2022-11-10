using AppServices.Tests.ModelsFake.Portfolio;
using AppServices.Validations.Portfolio;
using FluentValidation.TestHelper;

namespace AppServices.Tests.Validations.Portfolio;

public class UninvestmentCreateValidatorTests
{
    [Fact]
    public void Should_Pass_When_Run_InvestmentCreateValidator()
    {
        // Arrange
        var investmentFake = CreateUninvestmentModel.UninvestmentFake();
        var validator = new UninvestmentCreateValidator();

        // Act
        var result = validator.TestValidate(investmentFake);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ConvertedAt);
    }

    [Fact]
    public void Should_Fail_When_Run_InvestmentCreateValidator()
    {
        // Arrange
        var investmentFake = CreateUninvestmentModel.UninvestmentInvalid();
        var validator = new UninvestmentCreateValidator();

        // Act
        var result = validator.TestValidate(investmentFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConvertedAt);
    }
}
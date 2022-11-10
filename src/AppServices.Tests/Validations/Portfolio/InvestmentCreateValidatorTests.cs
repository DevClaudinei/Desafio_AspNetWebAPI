using Application.Models.Enum;
using AppServices.Tests.ModelsFake.Portfolio;
using AppServices.Validations.Portfolio;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace AppServices.Tests.Validations.Portfolio;

public class InvestmentCreateValidatorTests
{
    [Fact]
    public void Should_Pass_When_Run_InvestmentCreateValidator()
    {
        // Arrange
        var investmentFake = CreateInvestmentModel.InvestmentFake();
        var validator = new InvestmentCreateValidator();

        // Act
        var result = validator.TestValidate(investmentFake);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ConvertedAt);
    }

    [Fact]
    public void Should_Fail_When_Run_InvestmentCreateValidator()
    {
        // Arrange
        var investmentFake = CreateInvestmentModel.InvestmentInvalid();
        var validator = new InvestmentCreateValidator();

        // Act
        var result = validator.TestValidate(investmentFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConvertedAt);
    }
}
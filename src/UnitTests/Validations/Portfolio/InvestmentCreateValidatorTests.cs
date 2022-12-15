using AppServices.Validations.Portfolio;
using FluentValidation.TestHelper;
using UnitTests.EntitiesFake.Portfolios;

namespace UnitTests.Validations.Portfolio;

public class InvestmentCreateValidatorTests
{
    private readonly InvestmentCreateValidator _investmentCreateValidator = new InvestmentCreateValidator();

    [Fact]
    public void Should_Pass_When_Executing_InvestmentCreateValidator()
    {
        // Arrange
        var investmentFake = CreateInvestmentModel.InvestmentFake();
        
        // Act
        var result = _investmentCreateValidator.TestValidate(investmentFake);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ConvertedAt);
    }

    [Fact]
    public void Should_Fail_When_Executing_InvestmentCreateValidator()
    {
        // Arrange
        var investmentFake = CreateInvestmentModel.InvestmentInvalid();
        
        // Act
        var result = _investmentCreateValidator.TestValidate(investmentFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConvertedAt);
    }
}
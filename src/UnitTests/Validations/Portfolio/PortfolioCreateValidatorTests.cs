using Application.Models.Portfolio.Request;
using AppServices.Validations.Portfolio;
using FluentValidation.TestHelper;
using Moq;
using UnitTests.EntitiesFake.Portfolios;

namespace UnitTests.Validations.Portfolio;

public class PortfolioCreateValidatorTests
{
    private readonly PortfolioCreateValidator _portfolioCreateValidator = new PortfolioCreateValidator();

    [Fact]
    public void Should_Pass_When_Executing_PortfolioCreateValidator_When_CustomerId_Is_Valid()
    {
        // Arrange
        var portfolioFake = CreatePortfolioModel.PortfolioFake();
        
        // Act
        var result = _portfolioCreateValidator.TestValidate(portfolioFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Pass_When_Executing_PortfolioCreateValidator_When_CustomerId_Equal_Zero()
    {
        // Arrange
        var portfolioFake = new CreatePortfolioRequest(It.IsAny<long>());
        
        // Act
        var result = _portfolioCreateValidator.TestValidate(portfolioFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }
}
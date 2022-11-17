using Application.Models.Portfolio.Request;
using AppServices.Validations.Portfolio;
using Bogus;
using FluentValidation.TestHelper;
using UnitTests.EntitiesFake.Portfolios;

namespace UnitTests.Validations.Portfolio;

public class PortfolioCreateValidatorTests
{
    [Fact]
    public void Should_Pass_When_Executing_PortfolioCreateValidator_When_CustomerId_Is_Valid()
    {
        // Arrange
        var portfolioFake = CreatePortfolioModel.PortfolioFake();
        var validator = new PortfolioCreateValidator();

        // Act
        var result = validator.TestValidate(portfolioFake);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Pass_When_Executing_PortfolioCreateValidator_When_CustomerId_Equal_Zero()
    {
        // Arrange
        var portfolioFake = new Faker<CreatePortfolioRequest>("pt_BR")
            .CustomInstantiator(f => new CreatePortfolioRequest(
                f.Random.Long(0, 0)
            )).Generate();

        var validator = new PortfolioCreateValidator();

        // Act
        var result = validator.TestValidate(portfolioFake);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }
}
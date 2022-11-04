using Application.Models.Portfolio.Request;
using AppServices.Tests.ModelsFake.Portfolio;
using AppServices.Validations.Portfolio;
using Bogus;
using FluentAssertions;

namespace AppServices.Tests.Validations.Portfolio;

public class PortfolioFakeCreateValidator
{
    [Fact]
    public void Should_Verify_Create_When_CustomerId_Is_Valid()
    {
        // Arrange
        var portfolioFake = CreatePortfolioModel.PortfolioFake();
        var validator = new PortfolioCreateValidator();

        // Act
        var result = validator.Validate(portfolioFake);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_Verify_Create_When_CustomerId_Equal_Zero()
    {
        // Arrange
        var portfolioFake = new Faker<CreatePortfolioRequest>("pt_BR")
            .CustomInstantiator(f => new CreatePortfolioRequest(
                f.Random.Long(0, 0)
            )).Generate();
        
        var validator = new PortfolioCreateValidator();

        // Act
        var result = validator.Validate(portfolioFake);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }
}

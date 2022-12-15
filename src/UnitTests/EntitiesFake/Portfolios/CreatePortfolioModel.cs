using Application.Models.Portfolio.Request;
using Bogus;

namespace UnitTests.EntitiesFake.Portfolios;

public static class CreatePortfolioModel
{
    public static CreatePortfolioRequest PortfolioFake()
    {
        var portfolioFake = new Faker<CreatePortfolioRequest>("pt_BR")
            .CustomInstantiator(f => new CreatePortfolioRequest(
                customerId: f.Random.Long(1, 1)
            )).Generate();

        return portfolioFake;
    }
}
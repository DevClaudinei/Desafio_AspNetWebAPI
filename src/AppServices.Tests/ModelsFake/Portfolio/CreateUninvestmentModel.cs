using Application.Models.Enum;
using Application.Models.Portfolio.Request;
using Bogus;

namespace AppServices.Tests.ModelsFake.Portfolio;

public static class CreateUninvestmentModel
{
    public static UninvestimentRequest UninvestmentFake()
    {
        var investmentRequest = new Faker<UninvestimentRequest>("pt_BR")
            .CustomInstantiator(f => new UninvestimentRequest(
                ProductId: f.Random.Long(1, 1),
                PortfolioId: f.Random.Long(1, 1),
                Quotes: f.Random.Int(1, 10),
                ConvertedAt: DateTime.Now.AddDays(15),
                Direction: OrderDirection.Sell
            )).Generate();

        return investmentRequest;
    }

    public static UninvestimentRequest UninvestmentInvalid()
    {
        var investmentRequest = new Faker<UninvestimentRequest>("pt_BR")
            .CustomInstantiator(f => new UninvestimentRequest(
                ProductId: f.Random.Long(1, 1),
                PortfolioId: f.Random.Long(1, 1),
                Quotes: f.Random.Int(1, 10),
                ConvertedAt: DateTime.Now.AddDays(-15),
                Direction: OrderDirection.Sell
            )).Generate();

        return investmentRequest;
    }
}

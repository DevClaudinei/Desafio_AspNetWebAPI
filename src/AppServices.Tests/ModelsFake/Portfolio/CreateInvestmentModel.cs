using Application.Models.Enum;
using Application.Models.Portfolio.Request;
using Bogus;

namespace AppServices.Tests.ModelsFake.Portfolio;

public static class CreateInvestmentModel
{
	public static InvestmentRequest InvestmentFake()
	{
		var investmentRequest = new Faker<InvestmentRequest>("pt_BR")
			.CustomInstantiator(f => new InvestmentRequest(
				ProductId: f.Random.Long(1, 1),
				PortfolioId: f.Random.Long(1, 1),
                Quotes: f.Random.Int(1, 10),
				ConvertedAt: DateTime.Now.AddDays(-7),
                Direction: OrderDirection.Buy
            )).Generate();
        
		return investmentRequest;
	}

    public static InvestmentRequest InvestmentInvalid()
    {
        var investmentRequest = new Faker<InvestmentRequest>("pt_BR")
            .CustomInstantiator(f => new InvestmentRequest(
                ProductId: f.Random.Long(1, 1),
                PortfolioId: f.Random.Long(1, 1),
                Quotes: f.Random.Int(1, 10),
                ConvertedAt: f.Date.Future(),
                Direction: OrderDirection.Buy
            )).Generate();

        return investmentRequest;
    }
}
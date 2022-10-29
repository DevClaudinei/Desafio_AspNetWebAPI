using Application.Models.Portfolio.Request;
using Bogus;

namespace AppServices.Tests.ModelsFake.Portfolio;

public static class CreatePortfolioModel
{
	public static CreatePortfolioRequest PortfolioFake()
	{
		var createPortfolioFake = new Faker<CreatePortfolioRequest>("pt_BR")
			.CustomInstantiator(f => new CreatePortfolioRequest(
				f.Random.Long(1, 1)
			)).Generate();

		return createPortfolioFake;
	}
}
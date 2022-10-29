namespace Application.Models.Portfolio.Request;

public class CreatePortfolioRequest
{
	public CreatePortfolioRequest(long customerId)
	{
		CustomerId = customerId;
	}

    public long CustomerId { get; init; }
}
using Application.Models.Portfolio.Request;
using FluentValidation;

namespace AppServices.Validations.Portfolio;

public class PortfolioCreateValidator : AbstractValidator<CreatePortfolioRequest>
{
	public PortfolioCreateValidator()
	{
		RuleFor(x => x.CustomerId)
			.NotEmpty()
			.NotNull();
	}
}
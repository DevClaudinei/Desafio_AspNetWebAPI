using Application.Models.Portfolio.Request;
using FluentValidation;

namespace AppServices.Validations.Portfolio;

public class PortfolioCreateValidator : AbstractValidator<PortfolioCreate>
{
	public PortfolioCreateValidator()
	{
		RuleFor(x => x.CustomerId)
			.NotEmpty()
			.NotNull();

		RuleFor(x => x.Products)
			.NotEmpty()
			.NotNull();
	}
}

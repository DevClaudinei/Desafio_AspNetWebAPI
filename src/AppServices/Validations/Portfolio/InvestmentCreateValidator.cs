using Application.Models.Enum;
using Application.Models.Portfolio.Request;
using FluentValidation;
using System;

namespace AppServices.Validations.Portfolio;

public class InvestmentCreateValidator : AbstractValidator<InvestmentRequest>
{
	public InvestmentCreateValidator()
	{
		RuleFor(x => x.ProductId)
			.NotEmpty()
			.NotNull();

		RuleFor(x => x.PortfolioId)
            .NotEmpty()
            .NotNull();

		RuleFor(x => x.Quotes)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Direction)
            .NotEmpty()
            .NotNull()
			.Equal(OrderDirection.Buy);

		RuleFor(x => x.ConvertedAt)
			.NotEmpty()
			.NotNull()
			.When(x => x.Direction.Equals(OrderDirection.Buy))
			.LessThanOrEqualTo(DateTime.Now);
    }
}
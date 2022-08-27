using Application.Models.Portfolio.Request;
using FluentValidation;
using System;

namespace AppServices.Validations.Portfolio;

public class PortfolioUpdadeValidator : AbstractValidator<UpdatePortfolioRequest>
{
	public PortfolioUpdadeValidator()
	{
		//RuleFor(x => x.CustomerId)
		//	.NotEmpty();
	}
}

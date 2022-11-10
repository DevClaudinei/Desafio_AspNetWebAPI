using Application.Models.Enum;
using Application.Models.Portfolio.Request;
using FluentValidation;
using System;

namespace AppServices.Validations.Portfolio;

public class UninvestmentCreateValidator : AbstractValidator<UninvestimentRequest>
{
    public UninvestmentCreateValidator()
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
            .Equal(OrderDirection.Sell);

        RuleFor(x => x.ConvertedAt)
            .NotEmpty()
            .NotNull()
            .When(x => x.Direction.Equals(OrderDirection.Sell))
            .GreaterThanOrEqualTo(DateTime.Now);
    }
}
using Application.Models.Product.Request;
using FluentValidation;

namespace AppServices.Validations.Product;

public class ProductUpdateValidator : AbstractValidator<UpdateProductRequest>
{
    public ProductUpdateValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Quotes)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.UnitPrice)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.NetValue)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.ConvertedAt)
            .NotEmpty()
            .NotNull();
    }
}
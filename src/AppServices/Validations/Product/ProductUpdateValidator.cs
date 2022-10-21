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

        RuleFor(x => x.UnitPrice)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0);
    }
}
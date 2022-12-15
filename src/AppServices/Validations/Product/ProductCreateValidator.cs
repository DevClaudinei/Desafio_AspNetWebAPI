using Application.Models.Product.Request;
using FluentValidation;

namespace AppServices.Validations.Product;

public class ProductCreateValidator : AbstractValidator<CreateProductRequest>
{
    public ProductCreateValidator()
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
using Application.Models.Product.Request;
using FluentValidation;

namespace AppServices.Validations.Product;

public class ProductCreateValidation : AbstractValidator<CreateProductRequest>
{
    public ProductCreateValidation()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.UnitPrice)
            .NotEmpty()
            .NotNull();
    }
}
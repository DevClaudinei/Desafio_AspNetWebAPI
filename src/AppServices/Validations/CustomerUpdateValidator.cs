using Application.Models;
using FluentValidation;

namespace AppServices.Validations;
public class CustomerUpdateValidator : AbstractValidator<UpdateCustomerRequest>
{
    public CustomerUpdateValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress();

        RuleFor(x => x.EmailConfirmation)
            .NotEmpty()
            .NotNull()
            .EmailAddress();

        RuleFor(x => x)
        .Must(x => x.EmailConfirmation == x.Email)
        .WithMessage("Email e EmailConfirmation precisam ter informações identicas.");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsValidDocument());

        RuleFor(x => x.Cellphone)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsCellphone())
            .WithMessage("O Cellphone precisa estar no formato '(XX) XXXXX-XXXX'");

        RuleFor(x => x.Birthdate)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsReachedAdulthood())
            .WithMessage("Customer precisa ter 18 anos no minimo.");

        RuleFor(x => x.EmailSms)
            .NotNull();

        RuleFor(x => x.Whatsapp)
            .NotNull();

        RuleFor(x => x.Country)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.City)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsPostalCode())
            .WithMessage("O campo PostalCode deve estar no formato XXXXX-XXX");

        RuleFor(x => x.Address)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Number)
            .NotEmpty()
            .NotNull();
    }
}

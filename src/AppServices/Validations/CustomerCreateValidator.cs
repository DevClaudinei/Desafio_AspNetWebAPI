using Application.Models;
using FluentValidation;
using FluentValidation.Validators;

namespace AppServices.Validations;

public class CustomerCreateValidator : AbstractValidator<CreateCustomerRequest>
{
    public CustomerCreateValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .NotNull()
            .Must(x => x.Split(" ").Length > 1)
            .WithMessage("FullName não contém ao menos um sobrenome")
            .Must(x => !x.ContainsEmptySpace())
            .WithMessage("FullName contém espaços demasiados")
            .Must(x => !x.AnyInvalidLetter())
            .WithMessage("FullName contém caracteres especiais")
            .Must(x => x.PartsOfString())
            .WithMessage("FullName é inválido. Nome e/ou sobrenome devem conter duas letras ou mais.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress(EmailValidationMode.Net4xRegex);

        RuleFor(x => x)
            .Must(x => x.EmailConfirmation == x.Email)
            .WithMessage("Email e EmailConfirmation precisam ter informações idênticas.");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsValidDocument());

        RuleFor(x => x.CellPhone)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsCellphone())
            .WithMessage("O Cellphone precisa estar no formato '(XX) XXXXX-XXXX'");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsReachedAdulthood())
            .WithMessage("Customer precisa ter 18 anos no mínimo.");

        RuleFor(x => x.EmailSms)
            .NotNull();

        RuleFor(x => x.WhatsApp)
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
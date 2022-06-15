using Application.Models;
using FluentValidation;
using FluentValidation.Validators;

namespace AppServices.Validations;
public class CustomerUpdateValidator : AbstractValidator<UpdateCustomerRequest>
{
    public CustomerUpdateValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .NotNull()
            .Must(x => x.Split(" ").Length > 1)
            .WithMessage("FullName deve conter ao menos um sobrenome")
            .Must(x => !x.ContainsEmptySpace())
            .WithMessage("FullName não deve conter espaços em branco")
            .Must(x => !x.AnyInvalidLetter())
            .WithMessage("FullName não deve conter caracteres especiais")
            .Must(x => x.ValidateNumberOfCharactersInFirstAndLastName())
            .WithMessage("FullName inválido. Nome e/ou sobrenome devem conter ao menos duas letras ou mais");

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
            .Must(x => x.HasReachedAdulthood())
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

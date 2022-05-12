using FluentValidation;
using DesafioWarren.WebAPI.Models;

namespace DesafioWarren.WebAPI.Validations;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .NotNull()
            .WithMessage("FullName é um campo que não pode ser vazio ou nulo.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email é um campo que não pode ser vazio ou nulo.")
            .EmailAddress()
            .WithMessage("Formato do Email inválido! Email precisa estar num formato similar a nome@dominioemail.com");

        RuleFor(x => x.EmailConfirmation)
            .NotEmpty()
            .NotNull()
            .WithMessage("EmailConfirmation é um campo que não pode ser vazio ou nulo.")
            .EmailAddress()
            .WithMessage("Formato do EmailConfirmation inválido! Email precisa estar num formato similar a nome@dominioemail.com");

        RuleFor(x => x)
            .Must(x => x.EmailConfirmation == x.Email)
            .WithMessage("Email e EmailConfirmation precisam ter informações identicas.");

        RuleFor(x => x.Cpf)
            .NotEmpty()
            .NotNull()
            .WithMessage("Cpf é um campo que não pode ser vazio ou nulo.")
            .Must(x => x.IsValidDocument())
            .WithMessage("Documento inválido! CPF precisa estar no formato 'XXXXXXXXXXX'");

        RuleFor(x => x.Cellphone)
            .NotEmpty()
            .NotNull()
            .WithMessage("Cellphone é um campo que não pode ser vazio ou nulo.")
            .Must(x => x.IsCellphone())
            .WithMessage("O Cellphone precisa estar no formato '(XX) XXXXX-XXXX'");

        RuleFor(x => x.Birthdate)
            .NotEmpty()
            .NotNull()
            .WithMessage("Birthdate é um campo que não pode ser vazio ou nulo.")
            .Must(x => x.IsReachedAdulthood())
            .WithMessage("Customer precisa ter 18 anos no minimo.");
            // .LessThanOrEqualTo();

        RuleFor(x => x.EmailSms)
            .NotNull()
            .WithMessage("EmailSms é um campo que não pode ser vazio ou nulo.")
            .Must(x => x.IsBoolValid())
            .WithMessage("O campo EmailSms permite apenas 'true' ou 'false'");

        RuleFor(x => x.Whatsapp)
            .NotNull()
            .WithMessage("Whatsapp é um campo que não pode ser vazio ou nulo.")
            .Must(x => x.IsBoolValid())
            .WithMessage("O campo Whatsapp permite apenas 'true' ou 'false'");

        RuleFor(x => x.Country)
            .NotEmpty()
            .NotNull()
            .WithMessage("Country é um campo que não pode ser vazio ou nulo.");

        RuleFor(x => x.City)
            .NotEmpty()
            .NotNull()
            .WithMessage("City é um campo que não pode ser vazio ou nulo.");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .NotNull()
            .WithMessage("PostalCode é um campo que não pode ser vazio ou nulo.")
            .Must(x => x.IsPostalCode())
            .WithMessage("O campo PostalCode deve estar no formato XXXXX-XXX");

        RuleFor(x => x.Address)
            .NotEmpty()
            .NotNull()
            .WithMessage("Address é um campo que não pode ser vazio ou nulo.");

        RuleFor(x => x.Number)
            .NotEmpty()
            .NotNull()
            .WithMessage("Number é um campo que não pode ser vazio ou nulo.");
    }
}
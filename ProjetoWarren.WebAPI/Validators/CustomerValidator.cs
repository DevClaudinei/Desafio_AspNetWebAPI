using FluentValidation;
using SmartSchool.WebAPI.Models;

namespace ProjetoWarren.WebAPI.Validators
{

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.Id).NotEmpty().NotNull().WithMessage("Id é um campo que não pode ser nulo ou vazio.");

            RuleFor(c => c.FullName).NotEmpty().NotNull().WithMessage("FullName é um campo que não pode ser nulo.");

            RuleFor(c => c.Email).NotEmpty().NotNull().WithMessage("Email é um campo que não pode ser nulo.").EmailAddress().WithMessage("Formato do Email inválido! Email precisa estar num formato similar a nome@dominioemail.com");

            RuleFor(c => c.EmailConfirmation).NotEmpty().NotNull().WithMessage("EmailConfirmation é um campo que não pode ser nulo.").EmailAddress().WithMessage("Formato do EmailConfirmation inválido! Email precisa estar num formato similar a nome@dominioemail.com");
            /* .Must(em => em.isEqualEmail()).WithMessage("Os campos Email e EmailConfirmation precisam ser identicos.")); */

            RuleFor(c => c).Must(x => x.EmailConfirmation == x.Email).WithMessage("Email e EmailConfirmation precisam ter informações identicas.");

            RuleFor(c => c.Cpf).NotEmpty().NotNull().Must(doc => doc.isValidDocument()).WithMessage("Documento inválido! CPF precisa estar no formato 'XXXXXXXXXXX'");

            RuleFor(c => c.Cellphone).NotEmpty().NotNull().WithMessage("Cellphone é um campo que não pode ser vazio ou nulo.").Must(cel => cel.isCellphone()).WithMessage("O Cellphone precisa estar no formato '(XX) XXXXX-XXXX'");

            RuleFor(c => c.Birthdate).NotEmpty().NotNull().WithMessage("Birthdate é um campo que não pode ser nulo.");

            RuleFor(c => c.EmailSms)
            .NotNull()
            .WithMessage("EmailSms é um campo que não pode ser vazio ou nulo.")
            .Must(b => b.isBoolValid())
            .WithMessage("O campo EmailSms permite apenas 'true' ou 'false'");

            RuleFor(c => c.Whatsapp)
            .NotNull()
            .WithMessage("Whatsapp é um campo que não pode ser vazio ou nulo.")
            .Must(boo => boo.isBoolValid())
            .WithMessage("O campo Whatsapp permite apenas 'true' ou 'false'");

            RuleFor(c => c.Country).NotEmpty().NotNull().WithMessage("Country é um campo que não pode ser nulo.");

            RuleFor(c => c.City).NotEmpty().NotNull().WithMessage("City é um campo que não pode ser nulo.");

            RuleFor(c => c.PostalCode)
            .NotEmpty()
            .NotNull()
            .WithMessage("PostalCode é um campo que não pode ser nulo.")
            .Must(cep => cep.isPostalCode())
            .WithMessage("O campo PostalCode deve estar no formato XXXXX-XXX");

            RuleFor(c => c.Address).NotEmpty().NotNull().WithMessage("Address é um campo que não pode ser nulo.");

            RuleFor(c => c.Number)
            .NotEmpty()
            .NotNull()
            .WithMessage("Number é um campo que não pode ser nulo.");
        }
    }

}
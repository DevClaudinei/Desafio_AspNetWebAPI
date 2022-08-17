using Application.Models;
using FluentValidation;

namespace AppServices.Validations.CustomerBankInfo;

public class CustomerBankInfoCreateValidator : AbstractValidator<CreateCustomerBankInfoRequest>
{
    public CustomerBankInfoCreateValidator()
    {
        RuleFor(x => x.Account)
            .NotEmpty()
            .NotNull()
            .Must(x => x.IsValidAccount());

        RuleFor(x => x.AccountBalance)
            .NotEmpty()
            .NotNull();
    }
}

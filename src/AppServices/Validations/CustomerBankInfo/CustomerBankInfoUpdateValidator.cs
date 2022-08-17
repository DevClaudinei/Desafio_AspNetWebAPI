using Application.Models;
using FluentValidation;

namespace AppServices.Validations.CustomerBankInfo;

public class CustomerBankInfoUpdateValidator : AbstractValidator<UpdateCustomerBankInfoRequest>
{
    public CustomerBankInfoUpdateValidator()
    {
        //RuleFor(x => x.Account)
        //    .NotEmpty()
        //    .NotNull()
        //    .Must(x => x.IsValidAccount());

        RuleFor(x => x.AccountBalance)
            .NotEmpty()
            .NotNull();
    }
}

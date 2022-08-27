using Application.Models;
using FluentValidation;

namespace AppServices.Validations.CustomerBankInfo;

public class CustomerBankInfoUpdateValidator : AbstractValidator<UpdateCustomerBankInfoRequest>
{
    public CustomerBankInfoUpdateValidator()
    {
        RuleFor(x => x.AccountBalance)
            .NotEmpty()
            .NotNull();
    }
}

using Application.Models.CustomerBackInfo.Requests;
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
    }
}

using Application.Models.CustomerBackInfo.Requests;
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

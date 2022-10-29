using Application.Models.CustomerBackInfo.Requests;
using Bogus;

namespace AppServices.Tests.ModelsFake.CustomerBankInfo;

public static class CreateCustomerBankInfoModel
{
	public static CreateCustomerBankInfoRequest BankInfoFake()
	{
		var customerBankInfoFake = new Faker<CreateCustomerBankInfoRequest>("pt_BR")
			.CustomInstantiator(f => new CreateCustomerBankInfoRequest(
				account: f.Random.Guid().ToString(),
                customerId: f.Random.Int(1, 1)
			)).Generate();

		return customerBankInfoFake;
	}
}

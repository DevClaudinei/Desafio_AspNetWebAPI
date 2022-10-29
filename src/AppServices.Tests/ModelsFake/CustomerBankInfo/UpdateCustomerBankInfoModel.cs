using Application.Models.CustomerBackInfo.Requests;
using Bogus;

namespace AppServices.Tests.ModelsFake.CustomerBankInfo;

public static class UpdateCustomerBankInfoModel
{
	public static UpdateCustomerBankInfoRequest BankInfoFake()
	{
		var customerBankInfoFake = new Faker<UpdateCustomerBankInfoRequest>("pt_BR")
			.CustomInstantiator(f => new UpdateCustomerBankInfoRequest(
				account: f.Random.Guid().ToString(),
				accountBalance: f.Random.Decimal(0, 0),
				customerId: f.Random.Long(1, 1)
			)).Generate();

		return customerBankInfoFake;
	}
}

using Application.Models.CustomerBackInfo.Response;
using Bogus;

namespace AppServices.Tests.ModelsFake.CustomerBankInfo;

public static class CustomerBankInfoResponseModel
{

	public static CustomerBankInfoResult BankInfoFake()
	{
		var customerBankInfoResult = new Faker<CustomerBankInfoResult>("pt_BR")
			.CustomInstantiator(f => new CustomerBankInfoResult(
				id: 1,
				account: f.Random.Guid().ToString(),
				accountBalance: 0,
				customerId: 1
			)).Generate();

		return customerBankInfoResult;
	}
}
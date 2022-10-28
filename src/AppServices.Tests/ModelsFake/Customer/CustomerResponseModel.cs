using Application.Models.Customer.Response;
using Bogus;
using Bogus.Extensions.Brazil;

namespace AppServices.Tests.ModelsFake.Customer;

public static class CustomerResponseModel
{
    public static IEnumerable<CustomerResult> CustomerFakers(int quantity)
    {
        var createCustomersModelFake = new Faker<CustomerResult>("pt_BR")
            .CustomInstantiator(f => new CustomerResult(
                f.Random.Long(1, 1),
                f.Person.FullName,
                f.Internet.Email(),
                f.Person.Cpf(),
                f.Address.Country(),
                f.Address.City()
            )).Generate(quantity);

        return createCustomersModelFake;
    }
}
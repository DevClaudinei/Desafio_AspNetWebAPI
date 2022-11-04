using Application.Models.Customer.Requests;
using Bogus;
using Bogus.Extensions.Brazil;

namespace AppServices.Tests.ModelsFake.Customer;

public static class UpdateCustomerModel
{
    public static UpdateCustomerRequest CustomerFaker()
    {
        var createCustomerModelFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
                f.Person.FullName,
                f.Internet.Email(),
                f.Internet.Email(),
                f.Person.Cpf(false),
                f.Phone.PhoneNumberFormat(),
                f.Person.DateOfBirth,
                f.Random.Bool(),
                f.Random.Bool(),
                f.Address.Country(),
                f.Address.City(),
                f.Address.ZipCode(),
                f.Address.StreetAddress(),
                f.Random.Number(1, 10000)
            )).Generate();

        createCustomerModelFake.EmailConfirmation = createCustomerModelFake.Email;
        createCustomerModelFake.CellPhone = "(11) 98354-2892";

        return createCustomerModelFake;
    }
}
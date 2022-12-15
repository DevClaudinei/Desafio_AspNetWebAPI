using Application.Models.Customer.Requests;
using Bogus;
using Bogus.Extensions.Brazil;

namespace UnitTests.EntitiesFake.Customers;

public static class CreateCustomerModel
{
    public static CreateCustomerRequest CustomerFaker()
    {
        var createCustomerModelFake = new Faker<CreateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new CreateCustomerRequest(
                fullName: f.Person.FullName,
                email: f.Internet.Email(),
                emailConfirmation: f.Internet.Email(),
                cpf: f.Person.Cpf(false),
                cellphone: f.Phone.PhoneNumberFormat(),
                dateOfBirth: f.Person.DateOfBirth,
                emailSms: f.Random.Bool(),
                whatsApp: f.Random.Bool(),
                country: f.Address.Country(),
                city: f.Address.City(),
                postalCode: f.Address.ZipCode(),
                address: f.Address.StreetAddress(),
                number: f.Random.Number(1, 10000)
            )).Generate();

        createCustomerModelFake.EmailConfirmation = createCustomerModelFake.Email;
        createCustomerModelFake.Cellphone = "(11) 98354-2892";

        return createCustomerModelFake;
    }
}
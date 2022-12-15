using Application.Models.Customer.Requests;
using Bogus;
using Bogus.Extensions.Brazil;

namespace UnitTests.EntitiesFake.Customers;

public static class UpdateCustomerModel
{
    public static UpdateCustomerRequest CustomerFaker()
    {
        var createCustomerModelFake = new Faker<UpdateCustomerRequest>("pt_BR")
            .CustomInstantiator(f => new UpdateCustomerRequest(
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
        createCustomerModelFake.CellPhone = "(11) 98354-2892";

        return createCustomerModelFake;
    }
}
using Bogus;
using Bogus.Extensions.Brazil;
using DomainModels.Entities;
using System.Collections.Generic;

namespace UnitTests.EntitiesFake.Customers;

public static class CustomerFake
{
    public static Customer CustomerFaker()
    {
        var customerFake = new Faker<Customer>("pt_BR")
            .CustomInstantiator(f => new Customer(
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

        customerFake.EmailConfirmation = customerFake.Email;
        customerFake.Id = 1;

        return customerFake;
    }

    public static IEnumerable<Customer> CustomerFakers(int quantity)
    {
        var id = 1;
        var customersFakes = new Faker<Customer>("pt_BR")
            .CustomInstantiator(f => new Customer(
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
            )).Generate(quantity);

        foreach (var customerFake in customersFakes)
        {
            customerFake.Id = id++;
            customerFake.EmailConfirmation = customerFake.Email;
        }

        return customersFakes;
    }
}
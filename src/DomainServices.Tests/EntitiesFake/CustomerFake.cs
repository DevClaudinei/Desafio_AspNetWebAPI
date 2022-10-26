using Bogus;
using Bogus.Extensions.Brazil;
using DomainModels.Entities;

namespace DomainServices.Tests.EntitiesFake;

public static class CustomerFake
{
    public static Customer CustomerFaker()
    {
        var customerFake =  new Faker<Customer>("pt_BR")
            .CustomInstantiator(f => new Customer(
                f.Person.FullName,
                f.Internet.Email(),
                f.Internet.Email(),
                f.Person.Cpf(),
                f.Person.Phone,
                f.Person.DateOfBirth,
                f.Random.Bool(),
                f.Random.Bool(),
                f.Address.Country(),
                f.Address.City(),
                f.Address.ZipCode(),
                f.Address.StreetAddress(),
                f.Random.Number(1, 10000)
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
                f.Person.FullName,
                f.Internet.Email(),
                f.Internet.Email(),
                f.Person.Cpf(),
                f.Person.Phone,
                f.Person.DateOfBirth,
                f.Random.Bool(),
                f.Random.Bool(),
                f.Address.Country(),
                f.Address.City(),
                f.Address.ZipCode(),
                f.Address.StreetAddress(),
                f.Random.Number(1, 10000)
            )).Generate(quantity);

        foreach (var customerFake in customersFakes)
        {
            customerFake.Id = id ++;
            customerFake.EmailConfirmation = customerFake.Email;
        }
            
        return customersFakes;
    }
}

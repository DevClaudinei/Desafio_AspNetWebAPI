namespace Application.Models.Customer.Response;

public class CustomerResult
{
    public CustomerResult(
        long id,
        string fullName,
        string email,
        string cpf,
        string country,
        string city
    )
    {
        Id = id;
        FullName = fullName;
        Cpf = cpf;
        Email = email;
        Country = country;
        City = city;
    }

    public long Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Cpf { get; init; }
    public string Country { get; init; }
    public string City { get; init; }
}
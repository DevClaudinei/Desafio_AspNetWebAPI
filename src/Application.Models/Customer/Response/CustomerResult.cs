namespace Application.Models.Customer.Response;

public class CustomerResult
{
    public long Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Cpf { get; init; }
    public string Country { get; init; }
    public string City { get; init; }
}
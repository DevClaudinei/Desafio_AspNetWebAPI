using System;

namespace Application.Models;

public class CustomerResult
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Country { get; init; }
    public string City { get; init; }
}
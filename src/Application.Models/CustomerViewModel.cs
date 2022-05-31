using System;

namespace Application.Models;

public class CustomerViewModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
}
using System;

namespace Application.Models;

public class UpdateCustomerRequest
{
    public UpdateCustomerRequest() { }

    public UpdateCustomerRequest(
        string fullName,
        string email,
        string emailConfirmation,
        string cpf,
        string cellphone,
        DateTime dateOfBirth,
        bool emailSms,
        bool whatsApp,
        string country,
        string city,
        string postalCode,
        string address,
        int number
    )
    {
        FullName = fullName;
        Email = email;
        EmailConfirmation = emailConfirmation;
        Cpf = cpf;
        CellPhone = cellphone;
        DateOfBirth = dateOfBirth;
        EmailSms = emailSms;
        WhatsApp = whatsApp;
        Country = country;
        City = city;
        PostalCode = postalCode;
        Address = address;
        Number = number;
    }

    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string EmailConfirmation { get; set; }
    public string Cpf { get; set; }
    public string CellPhone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool EmailSms { get; set; }
    public bool WhatsApp { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public int Number { get; set; }
}
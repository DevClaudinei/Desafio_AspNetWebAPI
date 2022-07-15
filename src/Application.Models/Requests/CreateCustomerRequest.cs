using System;

namespace Application.Models;
public class CreateCustomerRequest
{
    public CreateCustomerRequest(
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
        Cellphone = cellphone;
        DateOfBirth = dateOfBirth;
        EmailSms = emailSms;
        WhatsApp = whatsApp;
        Country = country;
        City = city;
        PostalCode = postalCode;
        Address = address;
        Number = number;
    }

    public string FullName { get; }
    public string Email { get; }
    public string EmailConfirmation { get; }
    public string Cpf { get; }
    public string Cellphone { get; }
    public DateTime DateOfBirth { get; }
    public bool EmailSms { get; }
    public bool WhatsApp { get; }
    public string Country { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string Address { get; }
    public int Number { get; }
}
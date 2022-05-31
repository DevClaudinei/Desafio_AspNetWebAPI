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
        DateTime birthdate,
        bool emailSms,
        bool whatsapp,
        string country,
        string city,
        string postalCode,
        string address,
        int number
    )
    {
        this.FullName = fullName;
        this.Email = email;
        this.EmailConfirmation = emailConfirmation;
        this.Cpf = cpf;
        this.Cellphone = cellphone;
        this.Birthdate = birthdate;
        this.EmailSms = emailSms;
        this.Whatsapp = whatsapp;
        this.Country = country;
        this.City = city;
        this.PostalCode = postalCode;
        this.Address = address;
        this.Number = number;
    }

    public string FullName { get; set; }
    public string Email { get; set; }
    public string EmailConfirmation { get; set; }
    public string Cpf { get; set; }
    public string Cellphone { get; set; }
    public DateTime Birthdate { get; set; }
    public bool EmailSms { get; set; }
    public bool Whatsapp { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public int Number { get; set; }
}
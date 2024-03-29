using System;
using System.Collections.Generic;

namespace DomainModels.Entities;

public class Customer : IEntity
{
    protected Customer() { }

    public Customer(
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
        FullName = fullName.Trim();
        Email = email.Trim();
        EmailConfirmation = emailConfirmation.Trim();
        Cpf = cpf.Trim();
        Cellphone = cellphone;
        DateOfBirth = dateOfBirth;
        EmailSms = emailSms;
        WhatsApp = whatsApp;
        Country = country.Trim();
        City = city.Trim();
        Address = address.Trim();
        PostalCode = postalCode.Trim();
        Address = address.Trim();
        Number = number;
    }

    public string FullName { get; set; }
    public string Email { get; set; }
    public string EmailConfirmation { get; set; }
    public string Cpf { get; set; }
    public string Cellphone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool EmailSms { get; set; }
    public bool WhatsApp { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public int Number { get; set; }
    public virtual CustomerBankInfo CustomerBankInfo { get; set; }
    public virtual ICollection<Portfolio> Portfolios { get; set; }
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
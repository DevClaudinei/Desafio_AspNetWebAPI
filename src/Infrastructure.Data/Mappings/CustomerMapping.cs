using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainModels;

namespace Infrastructure.Data;

public class CustomerMapping : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id");

        builder.Property(x => x.FullName)
            .HasColumnName("Name");

        builder.Property(x => x.Email)
            .HasColumnName("Email");

        builder.Property(x => x.EmailConfirmation)
            .HasColumnName("EmailConfirmation");

        builder.Property(x => x.Cpf)
            .HasColumnName("Cpf");

        builder.Property(x => x.Cellphone)
            .HasColumnName("CellPhone");

        builder.Property(x => x.DateOfBirth)
            .HasColumnName("DateOfBirth");
        
        builder.Property(x => x.EmailSms)
            .HasColumnName("EmailSms");

        builder.Property(x => x.WhatsApp)
            .HasColumnName("WhatsApp");

        builder.Property(x => x.Country)
            .HasColumnName("Country");

        builder.Property(x => x.City)
            .HasColumnName("City");

        builder.Property(x => x.PostalCode)
            .HasColumnName("PostalCode");

        builder.Property(x => x.Address)
            .HasColumnName("Address");

        builder.Property(x => x.Number)
            .HasColumnName("Number");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt");

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("ModifiedAt");
        }
}
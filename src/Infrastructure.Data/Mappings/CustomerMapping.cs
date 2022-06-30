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
            .ValueGeneratedOnAdd()
            .HasColumnName("Id")
            .HasColumnType("Guid");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasColumnName("Name");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasColumnName("Email");

        builder.Property(x => x.EmailConfirmation)
            .IsRequired()
            .HasColumnName("EmailConfirmation");

        builder.Property(x => x.Cpf)
            .IsRequired()
            .HasColumnName("Cpf");

        builder.Property(x => x.Cellphone)
            .IsRequired()
            .HasColumnName("CellPhone");

        builder.Property(x => x.DateOfBirth)
            .IsRequired()
            .HasColumnName("DateOfBirth");
        
        builder.Property(x => x.EmailSms)
            .IsRequired()
            .HasColumnName("EmailSms");

        builder.Property(x => x.WhatsApp)
            .IsRequired()
            .HasColumnName("WhatsApp");

        builder.Property(x => x.Country)
            .IsRequired()
            .HasColumnName("Country");

        builder.Property(x => x.City)
            .IsRequired()
            .HasColumnName("City");

        builder.Property(x => x.PostalCode)
            .IsRequired()
            .HasColumnName("PostalCode");

        builder.Property(x => x.Address)
            .IsRequired()
            .HasColumnName("Address");

        builder.Property(x => x.Number)
            .IsRequired()
            .HasColumnName("Number");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("CreatedAt");

        builder.Property(x => x.ModifiedAt)
            .IsRequired()
            .HasColumnName("ModifiedAt");
        }
}
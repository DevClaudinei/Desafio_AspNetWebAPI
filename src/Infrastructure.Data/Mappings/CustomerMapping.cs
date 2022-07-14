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
            .HasColumnName("Id");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("FullName");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(70)
            .HasColumnName("Email");

        builder.Ignore(x => x.EmailConfirmation);

        builder.Property(x => x.Cpf)
            .IsRequired()
            .HasMaxLength(11)
            .HasColumnName("Cpf");

        builder.Property(x => x.Cellphone)
            .IsRequired()
            .HasMaxLength(15)
            .HasColumnName("Cellphone");

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
            .HasMaxLength(50)
            .HasColumnName("Country");

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("City");

        builder.Property(x => x.PostalCode)
            .IsRequired()
            .HasMaxLength(9)
            .HasColumnName("PostalCode");

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Address");

        builder.Property(x => x.Number)
            .IsRequired()
            .HasColumnName("Number");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("TIMESTAMP")
            .HasDefaultValueSql("CURRENT_TIMESTAMP()")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ModifiedAt)
            .HasColumnType("TIMESTAMP")
            .HasDefaultValueSql("CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()")
            .ValueGeneratedOnUpdate();
        }
}
using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Quotes)
            .IsRequired();

        builder.Property(x => x.NetValue)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(x => x.ConvertedAt)
            .IsRequired();
    }
}
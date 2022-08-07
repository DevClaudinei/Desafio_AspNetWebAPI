using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasOne(x => x.Portfolio)
            .WithMany(x => x.Products);

        builder.Property(x => x.Symbol)
            .IsRequired();

        builder.Property(x => x.Quotes)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .IsRequired();

        builder.Property(x => x.NetValue)
            .IsRequired();

        builder.Property(x => x.ConvertedAt)
            .IsRequired()
            .ValueGeneratedOnAdd();
    }
}

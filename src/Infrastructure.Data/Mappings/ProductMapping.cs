using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasMany(x => x.Portfolios)
            .WithMany(x => x.Products)
            .UsingEntity<PortfolioProduct>(
                pp => pp.HasOne(p => p.Portfolio)
                .WithMany().HasForeignKey(pp => pp.PortfolioId),
                pp => pp.HasOne(p => p.Product)
                .WithMany().HasForeignKey(pp => pp.ProductId)
            );

        builder.Property(x => x.Symbol)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();
    }
}
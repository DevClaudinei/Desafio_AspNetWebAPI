using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructure.Data.Mappings;

public class PortfolioProductMapping : IEntityTypeConfiguration<PortfolioProduct>
{
    public void Configure(EntityTypeBuilder<PortfolioProduct> builder)
    {
        builder.ToTable("PortfoliosProducts");

        builder.HasKey(x => new { x.PortfolioId, x.ProductId });

        builder.Property(x => x.PortfolioId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.HasOne(x => x.Portfolio)
            .WithMany(x => x.PortfoliosProducts)
            .HasForeignKey(x => x.PortfolioId);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.PortfoliosProducts)
            .HasForeignKey(x => x.ProductId);

    }
}

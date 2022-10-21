using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructure.Data.Mappings;

public class PortfolioProductMapping : IEntityTypeConfiguration<PortfolioProduct>
{
    public void Configure(EntityTypeBuilder<PortfolioProduct> builder)
    {
        builder.ToTable("PortfolioProducts");

        builder.HasKey(x => x.Id);

        builder.Property(a => a.PortfolioId)
            .IsRequired();

        builder.Property(a => a.ProductId)
            .IsRequired();

        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd();
    }
}
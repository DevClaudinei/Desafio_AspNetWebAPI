using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class PortfolioProductMapping : IEntityTypeConfiguration<PortfolioProduct>
{
    public void Configure(EntityTypeBuilder<PortfolioProduct> builder)
    {
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

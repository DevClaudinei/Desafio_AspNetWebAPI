using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class PortfolioMapping : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {
        builder.ToTable("Portfolios");

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasMany(x => x.Products)
            .WithMany(x => x.Portfolios)
            .UsingEntity<PortfolioProduct>(
               pp => pp.HasOne(p => p.Product)
               .WithMany().HasForeignKey(pp => pp.ProductId),
               pp => pp.HasOne(p => p.Portfolio)
               .WithMany().HasForeignKey(pp => pp.PortfolioId)
           );

        builder.Property(x => x.TotalBalance)
            .HasColumnType("decimal(10,2)");

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
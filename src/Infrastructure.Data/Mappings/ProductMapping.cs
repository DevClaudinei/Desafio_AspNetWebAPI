using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasMany(x => x.Portfolios)
            .WithMany(x => x.Products);
            
        //builder.HasOne(x => x.Portfolio)
        //    .WithMany(x => x.Products)
        //    .OnDelete(DeleteBehavior.ClientCascade);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Symbol)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Quotes)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .IsRequired();

        builder.Property(x => x.NetValue)
            .IsRequired();

        builder.Property(x => x.ConvertedAt)
            .IsRequired()
            .HasColumnType("TIMESTAMP")
            .HasDefaultValueSql("CURRENT_TIMESTAMP()")
            .ValueGeneratedOnAdd();
    }
}

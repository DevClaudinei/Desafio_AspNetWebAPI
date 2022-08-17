using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Infrastructure.Data.Mappings;

public class CustomerBankInfoMapping : IEntityTypeConfiguration<CustomerBankInfo>
{
    public void Configure(EntityTypeBuilder<CustomerBankInfo> builder)
    {
        builder.ToTable("CustomerBankInfos");

        builder.HasOne(x => x.Customer)
            .WithOne(x => x.CustomerBankInfo)
            .HasForeignKey<CustomerBankInfo>(x => x.CustomerId);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Account)
            .IsRequired();

        builder.Property(x => x.AccountBalance)
            //.HasDefaultValueSql("DECIMAL(65,2)")
            .IsRequired();

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

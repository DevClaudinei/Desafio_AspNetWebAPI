﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220812201838_Relacionamentos")]
    partial class Relacionamentos
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DomainModels.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("Id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasColumnName("Address");

                    b.Property<string>("Cellphone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)")
                        .HasColumnName("Cellphone");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("City");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Country");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)")
                        .HasColumnName("Cpf");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP()");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("DateOfBirth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("varchar(70)")
                        .HasColumnName("Email");

                    b.Property<bool>("EmailSms")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("EmailSms");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("FullName");

                    b.Property<DateTime?>("ModifiedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()");

                    b.Property<int>("Number")
                        .HasColumnType("int")
                        .HasColumnName("Number");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("varchar(9)")
                        .HasColumnName("PostalCode");

                    b.Property<bool>("WhatsApp")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("WhatsApp");

                    b.HasKey("Id");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("DomainModels.Entities.CustomerBankInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("AccountBalance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP()");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ModifiedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("CustomerBankInfos", (string)null);
                });

            modelBuilder.Entity("DomainModels.Entities.Portfolio", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP()");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("ModifiedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()");

                    b.Property<decimal>("TotalBalance")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Portfolios", (string)null);
                });

            modelBuilder.Entity("DomainModels.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ConvertedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP()");

                    b.Property<decimal>("NetValue")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Quotes")
                        .HasColumnType("int");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("PortfolioProduct", b =>
                {
                    b.Property<Guid>("PortfoliosId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ProductsId")
                        .HasColumnType("char(36)");

                    b.HasKey("PortfoliosId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("PortfolioProduct");
                });

            modelBuilder.Entity("DomainModels.Entities.CustomerBankInfo", b =>
                {
                    b.HasOne("DomainModels.Entities.Customer", "Customer")
                        .WithOne("CustomerBankInfo")
                        .HasForeignKey("DomainModels.Entities.CustomerBankInfo", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DomainModels.Entities.Portfolio", b =>
                {
                    b.HasOne("DomainModels.Entities.Customer", null)
                        .WithMany("Portfolios")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PortfolioProduct", b =>
                {
                    b.HasOne("DomainModels.Entities.Portfolio", null)
                        .WithMany()
                        .HasForeignKey("PortfoliosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainModels.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DomainModels.Entities.Customer", b =>
                {
                    b.Navigation("CustomerBankInfo");

                    b.Navigation("Portfolios");
                });
#pragma warning restore 612, 618
        }
    }
}
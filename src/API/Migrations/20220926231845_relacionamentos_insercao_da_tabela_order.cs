using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class relacionamentos_insercao_da_tabela_order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProduct_Portfolios_PortfolioId",
                table: "PortfolioProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProduct_Products_ProductId",
                table: "PortfolioProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioProduct",
                table: "PortfolioProduct");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioProduct_PortfolioId",
                table: "PortfolioProduct");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PortfolioProduct");

            migrationBuilder.DropColumn(
                name: "ConvertedAt",
                table: "PortfolioProduct");

            migrationBuilder.DropColumn(
                name: "NetValue",
                table: "PortfolioProduct");

            migrationBuilder.DropColumn(
                name: "Quotes",
                table: "PortfolioProduct");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "PortfolioProduct",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "PortfolioId",
                table: "PortfolioProduct",
                newName: "PortfoliosId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioProduct_ProductId",
                table: "PortfolioProduct",
                newName: "IX_PortfolioProduct_ProductsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioProduct",
                table: "PortfolioProduct",
                columns: new[] { "PortfoliosId", "ProductsId" });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Quotes = table.Column<int>(type: "int", nullable: false),
                    NetValue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ConvertedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PortfolioId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PortfolioId",
                table: "Order",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProductId",
                table: "Order",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioProduct_Portfolios_PortfoliosId",
                table: "PortfolioProduct",
                column: "PortfoliosId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioProduct_Products_ProductsId",
                table: "PortfolioProduct",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProduct_Portfolios_PortfoliosId",
                table: "PortfolioProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProduct_Products_ProductsId",
                table: "PortfolioProduct");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioProduct",
                table: "PortfolioProduct");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "PortfolioProduct",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "PortfoliosId",
                table: "PortfolioProduct",
                newName: "PortfolioId");

            migrationBuilder.RenameIndex(
                name: "IX_PortfolioProduct_ProductsId",
                table: "PortfolioProduct",
                newName: "IX_PortfolioProduct_ProductId");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "PortfolioProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConvertedAt",
                table: "PortfolioProduct",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "NetValue",
                table: "PortfolioProduct",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quotes",
                table: "PortfolioProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioProduct",
                table: "PortfolioProduct",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioProduct_PortfolioId",
                table: "PortfolioProduct",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioProduct_Portfolios_PortfolioId",
                table: "PortfolioProduct",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioProduct_Products_ProductId",
                table: "PortfolioProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

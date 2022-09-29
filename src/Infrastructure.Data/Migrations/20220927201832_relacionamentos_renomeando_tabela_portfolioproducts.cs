using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class relacionamentos_renomeando_tabela_portfolioproducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProduct_Portfolios_PortfoliosId",
                table: "PortfolioProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProduct_Products_ProductsId",
                table: "PortfolioProduct");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}

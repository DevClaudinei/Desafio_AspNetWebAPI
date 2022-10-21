using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class relacionamentos_removendo_lista_de_portfolioproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProducts_Portfolios_PortfolioId1",
                table: "PortfolioProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioProducts_Products_ProductId1",
                table: "PortfolioProducts");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioProducts_PortfolioId1",
                table: "PortfolioProducts");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioProducts_ProductId1",
                table: "PortfolioProducts");

            migrationBuilder.DropColumn(
                name: "PortfolioId1",
                table: "PortfolioProducts");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "PortfolioProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PortfolioId1",
                table: "PortfolioProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductId1",
                table: "PortfolioProducts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioProducts_PortfolioId1",
                table: "PortfolioProducts",
                column: "PortfolioId1");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioProducts_ProductId1",
                table: "PortfolioProducts",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioProducts_Portfolios_PortfolioId1",
                table: "PortfolioProducts",
                column: "PortfolioId1",
                principalTable: "Portfolios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioProducts_Products_ProductId1",
                table: "PortfolioProducts",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}

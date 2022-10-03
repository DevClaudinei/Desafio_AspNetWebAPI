using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class relacionamentos_adicionando_enum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PortfolioProducts_PortfolioProductId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PortfolioProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PortfolioProductId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Orders");

            migrationBuilder.AddColumn<long>(
                name: "PortfolioProductId",
                table: "Orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PortfolioProductId",
                table: "Orders",
                column: "PortfolioProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PortfolioProducts_PortfolioProductId",
                table: "Orders",
                column: "PortfolioProductId",
                principalTable: "PortfolioProducts",
                principalColumn: "Id");
        }
    }
}

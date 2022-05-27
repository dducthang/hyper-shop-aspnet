using Microsoft.EntityFrameworkCore.Migrations;

namespace HyperShop.DataAccess.Migrations
{
    public partial class AddOrderCityFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CityName",
                table: "Orders",
                column: "CityName");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cities_CityName",
                table: "Orders",
                column: "CityName",
                principalTable: "Cities",
                principalColumn: "CityName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cities_CityName",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CityName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

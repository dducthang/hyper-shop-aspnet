using Microsoft.EntityFrameworkCore.Migrations;

namespace HyperShop.DataAccess.Migrations
{
    public partial class AddStockTableFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stock_ColorId",
                table: "Stock",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_SizeId",
                table: "Stock",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Colors_ColorId",
                table: "Stock",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Sizes_SizeId",
                table: "Stock",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Colors_ColorId",
                table: "Stock");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Sizes_SizeId",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Stock_ColorId",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Stock_SizeId",
                table: "Stock");
        }
    }
}

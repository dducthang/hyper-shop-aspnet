using Microsoft.EntityFrameworkCore.Migrations;

namespace HyperShop.DataAccess.Migrations
{
    public partial class AddFKProduct_Stock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stock_ProductId",
                table: "Stock",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryImages_ColorId",
                table: "SecondaryImages",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryImages_ProductId",
                table: "SecondaryImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryImages_ColorId",
                table: "PrimaryImages",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryImages_ProductId",
                table: "PrimaryImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryImages_Colors_ColorId",
                table: "PrimaryImages",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryImages_Products_ProductId",
                table: "PrimaryImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryImages_Colors_ColorId",
                table: "SecondaryImages",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryImages_Products_ProductId",
                table: "SecondaryImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Products_ProductId",
                table: "Stock",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryImages_Colors_ColorId",
                table: "PrimaryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryImages_Products_ProductId",
                table: "PrimaryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryImages_Colors_ColorId",
                table: "SecondaryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryImages_Products_ProductId",
                table: "SecondaryImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Products_ProductId",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Stock_ProductId",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_SecondaryImages_ColorId",
                table: "SecondaryImages");

            migrationBuilder.DropIndex(
                name: "IX_SecondaryImages_ProductId",
                table: "SecondaryImages");

            migrationBuilder.DropIndex(
                name: "IX_PrimaryImages_ColorId",
                table: "PrimaryImages");

            migrationBuilder.DropIndex(
                name: "IX_PrimaryImages_ProductId",
                table: "PrimaryImages");
        }
    }
}

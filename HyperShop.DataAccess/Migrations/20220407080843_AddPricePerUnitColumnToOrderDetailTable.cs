using Microsoft.EntityFrameworkCore.Migrations;

namespace HyperShop.DataAccess.Migrations
{
    public partial class AddPricePerUnitColumnToOrderDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PricePerUnit",
                table: "OrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerUnit",
                table: "OrderDetails");
        }
    }
}

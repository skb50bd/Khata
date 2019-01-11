using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class RemodeledSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItem_Sales_SaleId",
                table: "LineItem");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "LineItem",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "NetPrice",
                table: "LineItem",
                newName: "Pricing_Retail");

            migrationBuilder.AlterColumn<int>(
                name: "SaleId",
                table: "LineItem",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<decimal>(
                name: "Pricing_Bulk",
                table: "LineItem",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Pricing_Margin",
                table: "LineItem",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Pricing_Purchase",
                table: "LineItem",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_LineItem_Sales_SaleId",
                table: "LineItem",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItem_Sales_SaleId",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Pricing_Bulk",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Pricing_Margin",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Pricing_Purchase",
                table: "LineItem");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "LineItem",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Pricing_Retail",
                table: "LineItem",
                newName: "NetPrice");

            migrationBuilder.AlterColumn<int>(
                name: "SaleId",
                table: "LineItem",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LineItem_Sales_SaleId",
                table: "LineItem",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

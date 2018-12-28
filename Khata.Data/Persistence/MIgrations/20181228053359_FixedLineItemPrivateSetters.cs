using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class FixedLineItemPrivateSetters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "LineItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LineItem",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetPrice",
                table: "LineItem",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "LineItem",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "LineItem",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LineItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "NetPrice",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LineItem");
        }
    }
}

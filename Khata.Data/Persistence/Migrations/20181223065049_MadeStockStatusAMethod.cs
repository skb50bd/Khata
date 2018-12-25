using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Migrations
{
    public partial class MadeStockStatusAMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Inventories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Inventories",
                nullable: false,
                defaultValue: "");
        }
    }
}

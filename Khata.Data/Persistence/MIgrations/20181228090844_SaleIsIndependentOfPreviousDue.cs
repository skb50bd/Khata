using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class SaleIsIndependentOfPreviousDue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payment_PreviousDebt",
                table: "Sales");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Payment_PreviousDebt",
                table: "Sales",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

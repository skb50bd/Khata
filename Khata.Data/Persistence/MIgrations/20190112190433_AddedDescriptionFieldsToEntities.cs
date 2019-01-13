using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class AddedDescriptionFieldsToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SupplierPayments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SalaryPayments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SalaryIssues",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DebtPayments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SupplierPayments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SalaryPayments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SalaryIssues");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DebtPayments");
        }
    }
}

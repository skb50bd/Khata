using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class AddedForeignKeyToDebtPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Customers_CustomerId",
                table: "DebtPayment");

            migrationBuilder.RenameColumn(
                name: "PreviousDebt",
                table: "DebtPayment",
                newName: "DebtBefore");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "DebtPayment",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Customers_CustomerId",
                table: "DebtPayment",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Customers_CustomerId",
                table: "DebtPayment");

            migrationBuilder.RenameColumn(
                name: "DebtBefore",
                table: "DebtPayment",
                newName: "PreviousDebt");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "DebtPayment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Customers_CustomerId",
                table: "DebtPayment",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

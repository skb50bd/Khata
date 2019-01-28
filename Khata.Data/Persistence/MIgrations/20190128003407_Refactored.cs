using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class Refactored : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_CashRegister_CashRegisterId",
                table: "Deposits");

            migrationBuilder.DropForeignKey(
                name: "FK_Withdrawals_CashRegister_CashRegisterId",
                table: "Withdrawals");

            migrationBuilder.DropIndex(
                name: "IX_Withdrawals_CashRegisterId",
                table: "Withdrawals");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_CashRegisterId",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "CashRegisterId",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "CashRegisterId",
                table: "Deposits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CashRegisterId",
                table: "Withdrawals",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CashRegisterId",
                table: "Deposits",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Withdrawals_CashRegisterId",
                table: "Withdrawals",
                column: "CashRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_CashRegisterId",
                table: "Deposits",
                column: "CashRegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_CashRegister_CashRegisterId",
                table: "Deposits",
                column: "CashRegisterId",
                principalTable: "CashRegister",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Withdrawals_CashRegister_CashRegisterId",
                table: "Withdrawals",
                column: "CashRegisterId",
                principalTable: "CashRegister",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

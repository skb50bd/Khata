using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class AddedTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposit_CashRegister_CashRegisterId",
                table: "Deposit");

            migrationBuilder.DropForeignKey(
                name: "FK_Withdrawal_CashRegister_CashRegisterId",
                table: "Withdrawal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Withdrawal",
                table: "Withdrawal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deposit",
                table: "Deposit");

            migrationBuilder.RenameTable(
                name: "Withdrawal",
                newName: "Withdrawals");

            migrationBuilder.RenameTable(
                name: "Deposit",
                newName: "Deposits");

            migrationBuilder.RenameIndex(
                name: "IX_Withdrawal_CashRegisterId",
                table: "Withdrawals",
                newName: "IX_Withdrawals_CashRegisterId");

            migrationBuilder.RenameIndex(
                name: "IX_Deposit_CashRegisterId",
                table: "Deposits",
                newName: "IX_Deposits_CashRegisterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Withdrawals",
                table: "Withdrawals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deposits",
                table: "Deposits",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_CashRegister_CashRegisterId",
                table: "Deposits");

            migrationBuilder.DropForeignKey(
                name: "FK_Withdrawals_CashRegister_CashRegisterId",
                table: "Withdrawals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Withdrawals",
                table: "Withdrawals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deposits",
                table: "Deposits");

            migrationBuilder.RenameTable(
                name: "Withdrawals",
                newName: "Withdrawal");

            migrationBuilder.RenameTable(
                name: "Deposits",
                newName: "Deposit");

            migrationBuilder.RenameIndex(
                name: "IX_Withdrawals_CashRegisterId",
                table: "Withdrawal",
                newName: "IX_Withdrawal_CashRegisterId");

            migrationBuilder.RenameIndex(
                name: "IX_Deposits_CashRegisterId",
                table: "Deposit",
                newName: "IX_Deposit_CashRegisterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Withdrawal",
                table: "Withdrawal",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deposit",
                table: "Deposit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposit_CashRegister_CashRegisterId",
                table: "Deposit",
                column: "CashRegisterId",
                principalTable: "CashRegister",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Withdrawal_CashRegister_CashRegisterId",
                table: "Withdrawal",
                column: "CashRegisterId",
                principalTable: "CashRegister",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

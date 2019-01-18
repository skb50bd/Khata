using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class AddedCashRegisterAndTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sales",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Purchases",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CashRegister",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Balance = table.Column<decimal>(nullable: false),
                    Metadata_Creator = table.Column<string>(nullable: true),
                    Metadata_CreationTime = table.Column<DateTimeOffset>(nullable: false),
                    Metadata_Modifier = table.Column<string>(nullable: true),
                    Metadata_ModificationTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashRegister", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deposit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    TableName = table.Column<string>(nullable: true),
                    RowId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CashRegisterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deposit_CashRegister_CashRegisterId",
                        column: x => x.CashRegisterId,
                        principalTable: "CashRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Withdrawal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    TableName = table.Column<string>(nullable: true),
                    RowId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CashRegisterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Withdrawal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Withdrawal_CashRegister_CashRegisterId",
                        column: x => x.CashRegisterId,
                        principalTable: "CashRegister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposit_CashRegisterId",
                table: "Deposit",
                column: "CashRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Withdrawal_CashRegisterId",
                table: "Withdrawal",
                column: "CashRegisterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposit");

            migrationBuilder.DropTable(
                name: "Withdrawal");

            migrationBuilder.DropTable(
                name: "CashRegister");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Purchases");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class AddedFunctionalityToSaleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Customers_CustomerId",
                table: "DebtPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Customers_CustomerId",
                table: "Sale");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sale",
                table: "Sale");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DebtPayment",
                table: "DebtPayment");

            migrationBuilder.RenameTable(
                name: "Sale",
                newName: "Sales");

            migrationBuilder.RenameTable(
                name: "DebtPayment",
                newName: "DebtPayments");

            migrationBuilder.RenameIndex(
                name: "IX_Sale_CustomerId",
                table: "Sales",
                newName: "IX_Sales_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_DebtPayment_CustomerId",
                table: "DebtPayments",
                newName: "IX_DebtPayments_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Sales",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Payment_DiscountCash",
                table: "Sales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Payment_Paid",
                table: "Sales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Payment_PreviousDebt",
                table: "Sales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Payment_SubTotal",
                table: "Sales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Sales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Metadata_CreationTime",
                table: "Sales",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Creator",
                table: "Sales",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Metadata_ModificationTime",
                table: "Sales",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Modifier",
                table: "Sales",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DebtPayments",
                table: "DebtPayments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LineItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SaleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineItem_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineItem_SaleId",
                table: "LineItem",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayments_Customers_CustomerId",
                table: "DebtPayments",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                table: "Sales",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayments_Customers_CustomerId",
                table: "DebtPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "LineItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DebtPayments",
                table: "DebtPayments");

            migrationBuilder.DropColumn(
                name: "Payment_DiscountCash",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Payment_Paid",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Payment_PreviousDebt",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Payment_SubTotal",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Metadata_CreationTime",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Metadata_Creator",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Metadata_ModificationTime",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Metadata_Modifier",
                table: "Sales");

            migrationBuilder.RenameTable(
                name: "Sales",
                newName: "Sale");

            migrationBuilder.RenameTable(
                name: "DebtPayments",
                newName: "DebtPayment");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_CustomerId",
                table: "Sale",
                newName: "IX_Sale_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_DebtPayments_CustomerId",
                table: "DebtPayment",
                newName: "IX_DebtPayment_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Sale",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sale",
                table: "Sale",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DebtPayment",
                table: "DebtPayment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Customers_CustomerId",
                table: "DebtPayment",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Customers_CustomerId",
                table: "Sale",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

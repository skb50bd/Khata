using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations.SQLServer
{
    public partial class AddedOutletInfoToInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Outlets_OutletId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Outlets_OutletId",
                table: "Sale");

            migrationBuilder.AlterColumn<int>(
                name: "OutletId",
                table: "Sale",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OutletId",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutletId",
                table: "Invoice",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_OutletId",
                table: "Invoice",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Outlets_OutletId",
                table: "Invoice",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Outlets_OutletId",
                table: "Products",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Outlets_OutletId",
                table: "Sale",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Outlets_OutletId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Outlets_OutletId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Outlets_OutletId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_OutletId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "OutletId",
                table: "Sale",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "OutletId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Outlets_OutletId",
                table: "Products",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Outlets_OutletId",
                table: "Sale",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

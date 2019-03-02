using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations.SQLServer
{
    public partial class OutletInCustomerInvoiceIsOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Outlets_OutletId",
                table: "Invoice");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Outlets_OutletId",
                table: "Invoice",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Outlets_OutletId",
                table: "Invoice");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Outlets_OutletId",
                table: "Invoice",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

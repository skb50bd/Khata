using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class AddedSavedSaleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Sales_SaleId",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleLineItem_Sales_SaleId",
                table: "SaleLineItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Invoice_InvoiceId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Metadata_MetadataId",
                table: "Sales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.RenameTable(
                name: "Sales",
                newName: "Sale");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_MetadataId",
                table: "Sale",
                newName: "IX_Sale_MetadataId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_InvoiceId",
                table: "Sale",
                newName: "IX_Sale_InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_CustomerId",
                table: "Sale",
                newName: "IX_Sale_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sale",
                table: "Sale",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Sale_SaleId",
                table: "Refunds",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Customers_CustomerId",
                table: "Sale",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Invoice_InvoiceId",
                table: "Sale",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Metadata_MetadataId",
                table: "Sale",
                column: "MetadataId",
                principalTable: "Metadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLineItem_Sale_SaleId",
                table: "SaleLineItem",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Sale_SaleId",
                table: "Refunds");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Customers_CustomerId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Invoice_InvoiceId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Metadata_MetadataId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleLineItem_Sale_SaleId",
                table: "SaleLineItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sale",
                table: "Sale");

            migrationBuilder.RenameTable(
                name: "Sale",
                newName: "Sales");

            migrationBuilder.RenameIndex(
                name: "IX_Sale_MetadataId",
                table: "Sales",
                newName: "IX_Sales_MetadataId");

            migrationBuilder.RenameIndex(
                name: "IX_Sale_InvoiceId",
                table: "Sales",
                newName: "IX_Sales_InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Sale_CustomerId",
                table: "Sales",
                newName: "IX_Sales_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Sales_SaleId",
                table: "Refunds",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLineItem_Sales_SaleId",
                table: "SaleLineItem",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Customers_CustomerId",
                table: "Sales",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Invoice_InvoiceId",
                table: "Sales",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Metadata_MetadataId",
                table: "Sales",
                column: "MetadataId",
                principalTable: "Metadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

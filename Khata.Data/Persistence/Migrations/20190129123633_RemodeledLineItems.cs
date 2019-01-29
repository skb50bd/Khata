using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class RemodeledLineItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseLineItem_Metadata_MetadataId",
                table: "PurchaseLineItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleLineItem_Metadata_MetadataId",
                table: "SaleLineItem");

            migrationBuilder.DropIndex(
                name: "IX_SaleLineItem_MetadataId",
                table: "SaleLineItem");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseLineItem_MetadataId",
                table: "PurchaseLineItem");

            migrationBuilder.DropColumn(
                name: "MetadataId",
                table: "SaleLineItem");

            migrationBuilder.DropColumn(
                name: "MetadataId",
                table: "PurchaseLineItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MetadataId",
                table: "SaleLineItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MetadataId",
                table: "PurchaseLineItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleLineItem_MetadataId",
                table: "SaleLineItem",
                column: "MetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseLineItem_MetadataId",
                table: "PurchaseLineItem",
                column: "MetadataId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseLineItem_Metadata_MetadataId",
                table: "PurchaseLineItem",
                column: "MetadataId",
                principalTable: "Metadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLineItem_Metadata_MetadataId",
                table: "SaleLineItem",
                column: "MetadataId",
                principalTable: "Metadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

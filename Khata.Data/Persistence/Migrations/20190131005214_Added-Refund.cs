using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class AddedRefund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefundId",
                table: "SaleLineItem",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MetadataId = table.Column<int>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    SaleId = table.Column<int>(nullable: false),
                    CashBack = table.Column<decimal>(nullable: false),
                    DebtRollback = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refunds_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Refunds_Metadata_MetadataId",
                        column: x => x.MetadataId,
                        principalTable: "Metadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refunds_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleLineItem_RefundId",
                table: "SaleLineItem",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_CustomerId",
                table: "Refunds",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_MetadataId",
                table: "Refunds",
                column: "MetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_SaleId",
                table: "Refunds",
                column: "SaleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLineItem_Refunds_RefundId",
                table: "SaleLineItem",
                column: "RefundId",
                principalTable: "Refunds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleLineItem_Refunds_RefundId",
                table: "SaleLineItem");

            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_SaleLineItem_RefundId",
                table: "SaleLineItem");

            migrationBuilder.DropColumn(
                name: "RefundId",
                table: "SaleLineItem");
        }
    }
}

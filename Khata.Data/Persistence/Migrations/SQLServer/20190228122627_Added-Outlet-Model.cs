using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations.SQLServer
{
    public partial class AddedOutletModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutletId",
                table: "Sale",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutletId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Outlets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MetadataId = table.Column<int>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Slogan = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outlets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outlets_Metadata_MetadataId",
                        column: x => x.MetadataId,
                        principalTable: "Metadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_OutletId",
                table: "Sale",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OutletId",
                table: "Products",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_MetadataId",
                table: "Outlets",
                column: "MetadataId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Outlets_OutletId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Outlets_OutletId",
                table: "Sale");

            migrationBuilder.DropTable(
                name: "Outlets");

            migrationBuilder.DropIndex(
                name: "IX_Sale_OutletId",
                table: "Sale");

            migrationBuilder.DropIndex(
                name: "IX_Products_OutletId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "Products");
        }
    }
}

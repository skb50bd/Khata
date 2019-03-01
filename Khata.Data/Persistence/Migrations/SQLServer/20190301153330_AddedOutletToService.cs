using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations.SQLServer
{
    public partial class AddedOutletToService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutletId",
                table: "Services",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Services_OutletId",
                table: "Services",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Outlets_OutletId",
                table: "Services",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Outlets_OutletId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_OutletId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "Services");
        }
    }
}

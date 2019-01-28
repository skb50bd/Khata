using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Persistence.Migrations
{
    public partial class AddedMetadataToTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RowId",
                table: "Withdrawals",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Metadata_CreationTime",
                table: "Withdrawals",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Creator",
                table: "Withdrawals",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Metadata_ModificationTime",
                table: "Withdrawals",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Modifier",
                table: "Withdrawals",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RowId",
                table: "Deposits",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Metadata_CreationTime",
                table: "Deposits",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Creator",
                table: "Deposits",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Metadata_ModificationTime",
                table: "Deposits",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Metadata_Modifier",
                table: "Deposits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metadata_CreationTime",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "Metadata_Creator",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "Metadata_ModificationTime",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "Metadata_Modifier",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "Metadata_CreationTime",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "Metadata_Creator",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "Metadata_ModificationTime",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "Metadata_Modifier",
                table: "Deposits");

            migrationBuilder.AlterColumn<int>(
                name: "RowId",
                table: "Withdrawals",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RowId",
                table: "Deposits",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

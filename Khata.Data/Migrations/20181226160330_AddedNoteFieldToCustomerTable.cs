using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Migrations
{
    public partial class AddedNoteFieldToCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Debt",
                table: "Customers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Customers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 771, DateTimeKind.Unspecified).AddTicks(7975), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(1612), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4575), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4586), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4609), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4610), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4612), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4613), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4614), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4616), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4618), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4619), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4620), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4621), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4623), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4624), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4625), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4626), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4628), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4629), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4630), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4631), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4633), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4634), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4635), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4636), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4638), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4639), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4640), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4642), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4643), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4644), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4646), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4647), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4648), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4649), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4651), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4652), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4653), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4654), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4656), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4657), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4658), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4659), new TimeSpan(0, 6, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Debt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 676, DateTimeKind.Unspecified).AddTicks(8775), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 679, DateTimeKind.Unspecified).AddTicks(8516), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(807), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(814), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(833), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(835), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(836), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(837), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(840), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(841), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(842), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(844), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(845), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(847), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(848), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(849), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(851), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(852), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(853), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(854), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(856), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(857), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(858), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(859), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(861), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(862), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(863), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(865), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(866), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(867), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(868), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(870), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(871), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(872), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(874), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(875), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(876), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(877), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(879), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(880), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(881), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(882), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(884), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 18, 12, 11, 680, DateTimeKind.Unspecified).AddTicks(885), new TimeSpan(0, 6, 0, 0, 0)) });
        }
    }
}

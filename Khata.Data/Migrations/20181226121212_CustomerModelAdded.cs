using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Migrations
{
    public partial class CustomerModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsRemoved = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Balance = table.Column<decimal>(nullable: false),
                    Metadata_Creator = table.Column<string>(nullable: true),
                    Metadata_CreationTime = table.Column<DateTimeOffset>(nullable: false),
                    Metadata_Modifier = table.Column<string>(nullable: true),
                    Metadata_ModificationTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DebtPayment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsRemoved = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebtPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DebtPayment_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsRemoved = table.Column<bool>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sale_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_DebtPayment_CustomerId",
                table: "DebtPayment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CustomerId",
                table: "Sale",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DebtPayment");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 45, DateTimeKind.Unspecified).AddTicks(8310), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(2946), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5080), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5087), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5107), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5109), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5110), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5112), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5113), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5114), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5116), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5117), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5118), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5120), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5121), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5123), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5124), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5125), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5126), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5128), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5129), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5130), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5132), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5133), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5135), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5136), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5137), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5139), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5140), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5142), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5143), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5144), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5146), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5147), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5148), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5149), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5151), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5152), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5153), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5155), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5156), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5157), new TimeSpan(0, 6, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Metadata_CreationTime", "Metadata_ModificationTime" },
                values: new object[] { new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5159), new TimeSpan(0, 6, 0, 0, 0)), new DateTimeOffset(new DateTime(2018, 12, 26, 16, 17, 59, 48, DateTimeKind.Unspecified).AddTicks(5160), new TimeSpan(0, 6, 0, 0, 0)) });
        }
    }
}

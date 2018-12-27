using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Khata.Data.Migrations
{
    public partial class IgnoreDebtInCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DropColumn(
                name: "Debt",
                table: "Customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Debt",
                table: "Customers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "IsRemoved", "Name", "Unit", "Inventory_AlertAt", "Inventory_Stock", "Inventory_Warehouse", "Metadata_CreationTime", "Metadata_Creator", "Metadata_ModificationTime", "Metadata_Modifier", "Price_Bulk", "Price_Margin", "Price_Purchase", "Price_Retail" },
                values: new object[,]
                {
                    { 1, "Great Budget Phone", false, "Nokia 1100", null, 10m, 5m, 0m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 771, DateTimeKind.Unspecified).AddTicks(7975), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(1612), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 20, "Unstoppable growth!", false, "Google Pixel 3", null, 30m, 15m, 10m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4653), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4654), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 19, "Convinced by the camera!", false, "Google Pixel 2", null, 80m, 61m, 11m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4651), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4652), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 18, "Made by Google!", false, "Google Pixel", null, 100m, 51m, 50m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4648), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4649), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 17, "Keep the handy qwerty pad!", false, "Sony Xperia Ray", null, 101m, 51m, 1011m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4646), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4647), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 16, "Innovation at your fingertips.", false, "Sony Xperia Sola", null, 9m, 16m, 16m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4643), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4644), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 15, "Hello! Is it me you're looking for?", false, "Sony Xperia Z5 Premium", null, 10m, 5m, 20m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4640), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4642), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 14, "When being water resistant is not enough ;)", false, "Sony Xperia Z2", null, 10m, 5m, 0m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4638), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4639), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 13, "You know it's good, cause it's waterproof!", false, "Sony Xperia Z", null, 10m, 6m, 0m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4635), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4636), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 12, "Feel the Life", false, "Okapia Life", null, 100m, 0m, 0m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4633), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4634), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 11, "May the force be with you.", false, "Moto Z3 Force", null, 100m, 6m, 0m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4630), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4631), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 10, "Ya didnot expect that, did ya?", false, "OnePlus 5", null, 80m, 6m, 7m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4628), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4629), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 9, "Cause OnePlus 3S would be dumb.", false, "OnePlus 3T", null, 3m, 3m, 3m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4625), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4626), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 8, "Cool, innit?", false, "OnePlus 6T", null, 18m, 20m, 5m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4623), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4624), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 7, "Feel the power in you palm.", false, "Samsung Galaxy S9+", null, 1000m, 15m, 550m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4620), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4621), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 6, "You're gonna miss it when its gone.", false, "Nokia 3310", null, 5m, 0m, 5m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4618), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4619), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 5, "Thought you'd never have something like it?", false, "Moto X Play", null, 10m, 18m, 40m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4614), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4616), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 4, "Awesome Battery Life", false, "Moto Z Play", null, 10m, 0m, 0m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4612), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4613), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 3, "Show you what we can do.", false, "Nokia X6", null, 10000m, 55m, 550m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4609), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4610), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 2, "Killer Phone", false, "Moto 360", null, 10m, 15m, 10m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4575), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4586), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 21, "It's apple!", false, "Apple iPhone X", null, 100m, 50m, 0m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4656), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4657), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m },
                    { 22, "Cause we need more money!", false, "Apple iPhone X Max", null, 10m, 3m, 3m, new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4658), new TimeSpan(0, 6, 0, 0, 0)), "admin", new DateTimeOffset(new DateTime(2018, 12, 26, 22, 3, 29, 775, DateTimeKind.Unspecified).AddTicks(4659), new TimeSpan(0, 6, 0, 0, 0)), "admin", 1850m, 1600m, 1500m, 2000m }
                });
        }
    }
}

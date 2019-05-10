﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Persistence.Migrations.SQLServer
{
    public partial class AddedDateFieldsToPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaymentDate",
                table: "SupplierPayments",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaymentDate",
                table: "SalaryPayments",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaymentDate",
                table: "DebtPayments",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "SupplierPayments");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "SalaryPayments");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "DebtPayments");
        }
    }
}

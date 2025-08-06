using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "Id", "CreateDate", "Description", "Name", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 28, 11, 13, 50, 542, DateTimeKind.Local).AddTicks(9630), "Ekleme İşlemi", "Add", true },
                    { 2, new DateTime(2025, 7, 28, 11, 13, 50, 542, DateTimeKind.Local).AddTicks(9631), "Güncelleme İşlemi", "Update", true },
                    { 3, new DateTime(2025, 7, 28, 11, 13, 50, 542, DateTimeKind.Local).AddTicks(9632), "Silme İşlemi", "Delete", true }
                });

            migrationBuilder.InsertData(
                table: "PolicyTypes",
                columns: new[] { "Id", "CreateDate", "Description", "Name", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 28, 11, 13, 50, 542, DateTimeKind.Local).AddTicks(9399), null, "Trafik", false },
                    { 2, new DateTime(2025, 7, 28, 11, 13, 50, 542, DateTimeKind.Local).AddTicks(9413), null, "Kasko", false },
                    { 3, new DateTime(2025, 7, 28, 11, 13, 50, 542, DateTimeKind.Local).AddTicks(9413), null, "Dask", false },
                    { 4, new DateTime(2025, 7, 28, 11, 13, 50, 542, DateTimeKind.Local).AddTicks(9414), null, "Konut", false },
                    { 5, new DateTime(2025, 7, 28, 11, 13, 50, 542, DateTimeKind.Local).AddTicks(9415), null, "İş Yeri", false },
                    { 6, new DateTime(2025, 7, 28, 11, 13, 50, 542, DateTimeKind.Local).AddTicks(9416), null, "Sağlık", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Actions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Actions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Actions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PolicyTypes",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}

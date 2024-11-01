using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagament_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddedAnswerToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 26, 20, 55, 56, 329, DateTimeKind.Local).AddTicks(9240));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 24, 22, 38, 29, 121, DateTimeKind.Local).AddTicks(3140));
        }
    }
}

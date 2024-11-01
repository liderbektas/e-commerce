using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagament_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmailModel10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionsId",
                table: "Answers");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionsId",
                table: "Answers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 29, 12, 30, 50, 486, DateTimeKind.Local).AddTicks(1620));

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionsId",
                table: "Answers",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionsId",
                table: "Answers");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionsId",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 28, 14, 8, 6, 547, DateTimeKind.Local).AddTicks(4320));

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionsId",
                table: "Answers",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

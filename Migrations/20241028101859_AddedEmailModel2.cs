using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagament_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmailModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionsId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionsId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuestionsId",
                table: "Answers");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 28, 13, 18, 58, 937, DateTimeKind.Local).AddTicks(7150));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionsId",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 10, 28, 13, 17, 38, 377, DateTimeKind.Local).AddTicks(4780));

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionsId",
                table: "Answers",
                column: "QuestionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionsId",
                table: "Answers",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}

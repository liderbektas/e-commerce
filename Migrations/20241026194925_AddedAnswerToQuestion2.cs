using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagament_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddedAnswerToQuestion2 : Migration
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
                value: new DateTime(2024, 10, 26, 22, 49, 25, 87, DateTimeKind.Local).AddTicks(7060));

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers");

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
                value: new DateTime(2024, 10, 26, 22, 31, 9, 76, DateTimeKind.Local).AddTicks(6450));

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

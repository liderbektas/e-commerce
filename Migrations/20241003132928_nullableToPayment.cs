using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagament_MVC.Migrations
{
    /// <inheritdoc />
    public partial class nullableToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Bank_BankId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CreditCart_CreditCartId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CreditCartId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Bank_BankId",
                table: "Orders",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CreditCart_CreditCartId",
                table: "Orders",
                column: "CreditCartId",
                principalTable: "CreditCart",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Bank_BankId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CreditCart_CreditCartId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CreditCartId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BankId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Bank_BankId",
                table: "Orders",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CreditCart_CreditCartId",
                table: "Orders",
                column: "CreditCartId",
                principalTable: "CreditCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

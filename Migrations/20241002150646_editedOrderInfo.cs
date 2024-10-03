using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagament_MVC.Migrations
{
    /// <inheritdoc />
    public partial class editedOrderInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingCountry",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingFullName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingState",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingStreet",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingZipCode",
                table: "Orders",
                newName: "address");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CVV",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastDate",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVV",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastDate",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Orders",
                newName: "ShippingZipCode");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingCity",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingCountry",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingFullName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingState",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingStreet",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

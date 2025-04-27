using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunaSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstVerificationSendColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 28, 3, 9, 47, 633, DateTimeKind.Utc).AddTicks(9210),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 28, 2, 20, 28, 618, DateTimeKind.Utc).AddTicks(9060));

            migrationBuilder.AddColumn<bool>(
                name: "FirstVerificationCodeSend",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 28, 3, 9, 47, 633, DateTimeKind.Utc).AddTicks(7060),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 28, 2, 20, 28, 618, DateTimeKind.Utc).AddTicks(7010));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstVerificationCodeSend",
                table: "User");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 28, 2, 20, 28, 618, DateTimeKind.Utc).AddTicks(9060),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 28, 3, 9, 47, 633, DateTimeKind.Utc).AddTicks(9210));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 28, 2, 20, 28, 618, DateTimeKind.Utc).AddTicks(7010),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 28, 3, 9, 47, 633, DateTimeKind.Utc).AddTicks(7060));
        }
    }
}

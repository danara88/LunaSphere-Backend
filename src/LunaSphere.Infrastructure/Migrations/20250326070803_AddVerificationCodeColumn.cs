using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunaSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationCodeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "VerificationTokenExpires",
                table: "User",
                newName: "VerificationCodeExpires");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 26, 7, 8, 3, 285, DateTimeKind.Utc).AddTicks(9880),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 2, 14, 20, 39, 50, 200, DateTimeKind.Utc).AddTicks(840));

            migrationBuilder.AddColumn<short>(
                name: "VerificationCode",
                table: "User",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 26, 7, 8, 3, 285, DateTimeKind.Utc).AddTicks(7880),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 2, 14, 20, 39, 50, 199, DateTimeKind.Utc).AddTicks(8680));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "VerificationCodeExpires",
                table: "User",
                newName: "VerificationTokenExpires");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 14, 20, 39, 50, 200, DateTimeKind.Utc).AddTicks(840),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 26, 7, 8, 3, 285, DateTimeKind.Utc).AddTicks(9880));

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "User",
                type: "text",
                unicode: false,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 2, 14, 20, 39, 50, 199, DateTimeKind.Utc).AddTicks(8680),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 26, 7, 8, 3, 285, DateTimeKind.Utc).AddTicks(7880));
        }
    }
}

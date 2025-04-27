using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunaSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixTypoIssueExpiresAtRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExperiesAt",
                table: "RefreshToken",
                newName: "ExpiresAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 2, 21, 18, 17, 482, DateTimeKind.Utc).AddTicks(9710),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 28, 3, 9, 47, 633, DateTimeKind.Utc).AddTicks(9210));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 2, 21, 18, 17, 482, DateTimeKind.Utc).AddTicks(7460),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 3, 28, 3, 9, 47, 633, DateTimeKind.Utc).AddTicks(7060));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "RefreshToken",
                newName: "ExperiesAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 28, 3, 9, 47, 633, DateTimeKind.Utc).AddTicks(9210),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 4, 2, 21, 18, 17, 482, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 3, 28, 3, 9, 47, 633, DateTimeKind.Utc).AddTicks(7060),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 4, 2, 21, 18, 17, 482, DateTimeKind.Utc).AddTicks(7460));
        }
    }
}

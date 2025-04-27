using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunaSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLastVerificationEmailSendField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 11, 0, 18, 41, 450, DateTimeKind.Utc).AddTicks(7040),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 4, 6, 19, 42, 17, 209, DateTimeKind.Utc).AddTicks(8120));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastVerificationEmailSent",
                table: "User",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 11, 0, 18, 41, 450, DateTimeKind.Utc).AddTicks(4570),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 4, 6, 19, 42, 17, 209, DateTimeKind.Utc).AddTicks(5560));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastVerificationEmailSent",
                table: "User");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 6, 19, 42, 17, 209, DateTimeKind.Utc).AddTicks(8120),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 4, 11, 0, 18, 41, 450, DateTimeKind.Utc).AddTicks(7040));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 4, 6, 19, 42, 17, 209, DateTimeKind.Utc).AddTicks(5560),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 4, 11, 0, 18, 41, 450, DateTimeKind.Utc).AddTicks(4570));
        }
    }
}

#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPIApplication.Migrations.AuthModel._202312010940;

/// <inheritdoc />
public partial class editUserPasswordMaxLength : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            "Password",
            "Users",
            "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(24)",
            oldMaxLength: 24);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            "Password",
            "Users",
            "nvarchar(24)",
            maxLength: 24,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50);
    }
}
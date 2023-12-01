#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPIApplication.Migrations.AuthModel._202311281656;

/// <inheritdoc />
public partial class changeAuthKeyId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Permissions",
            table => new
            {
                PermissionsId = table.Column<string>("nvarchar(450)", nullable: false),
                ControllerName = table.Column<string>("nvarchar(60)", maxLength: 60, nullable: false),
                Routers = table.Column<string>("nvarchar(max)", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Permissions", x => x.PermissionsId); });

        migrationBuilder.CreateTable(
            "Users",
            table => new
            {
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                UserName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                Password = table.Column<string>("nvarchar(24)", maxLength: 24, nullable: false),
                Email = table.Column<string>("nvarchar(max)", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Users", x => x.UserId); });

        migrationBuilder.CreateTable(
            "UserPermissions",
            table => new
            {
                UserId = table.Column<string>("nvarchar(450)", nullable: false),
                PermissionsId = table.Column<string>("nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserPermissions", x => new { x.UserId, x.PermissionsId });
                table.ForeignKey(
                    "FK_UserPermissions_Permissions_PermissionsId",
                    x => x.PermissionsId,
                    "Permissions",
                    "PermissionsId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_UserPermissions_Users_UserId",
                    x => x.UserId,
                    "Users",
                    "UserId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_UserPermissions_PermissionsId",
            "UserPermissions",
            "PermissionsId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "UserPermissions");

        migrationBuilder.DropTable(
            "Permissions");

        migrationBuilder.DropTable(
            "Users");
    }
}
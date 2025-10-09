using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.Migrations
{
    /// <inheritdoc />
    public partial class DropTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.AddColumn<string>(
                name: "Rule",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rule",
                table: "users");

            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    adminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSlat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    adminEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    adminName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.adminID);
                    table.CheckConstraint("CK_Admins_Email_Gmail", "[adminEmail] LIKE '%@%.com'");
                });

            migrationBuilder.CreateIndex(
                name: "IX_admins_adminEmail",
                table: "admins",
                column: "adminEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_admins_adminName",
                table: "admins",
                column: "adminName",
                unique: true);
        }
    }
}

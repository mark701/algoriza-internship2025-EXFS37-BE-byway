using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.Migrations
{
    /// <inheritdoc />
    public partial class Algoriza4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Course_Rate_0<x<=5",
                table: "courses");

            migrationBuilder.AlterColumn<int>(
                name: "courseRate",
                table: "courses",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,1)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Course_Rate_0<x<=5",
                table: "instructors",
                sql: "[courseRate] > 0 AND [courseRate] <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Course_Rate_0<x<=5",
                table: "instructors");

            migrationBuilder.AlterColumn<decimal>(
                name: "courseRate",
                table: "courses",
                type: "decimal(2,1)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Course_Rate_0<x<=5",
                table: "courses",
                sql: "[courseRate] > 0 AND [courseRate] <= 5");
        }
    }
}

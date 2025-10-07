using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.Migrations
{
    /// <inheritdoc />
    public partial class Algoriza5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Course_Rate_0<x<=5",
                table: "instructors");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Course_Rate_0<x<=51",
                table: "instructors",
                sql: "[courseRate] > 0 AND [courseRate] <= 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Course_Rate_0<x<=5",
                table: "courses",
                sql: "[courseRate] > 0 AND [courseRate] <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Course_Rate_0<x<=51",
                table: "instructors");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Course_Rate_0<x<=5",
                table: "courses");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Course_Rate_0<x<=5",
                table: "instructors",
                sql: "[courseRate] > 0 AND [courseRate] <= 5");
        }
    }
}

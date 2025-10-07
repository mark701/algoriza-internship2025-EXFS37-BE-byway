using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.Migrations
{
    /// <inheritdoc />
    public partial class Algoriza1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    adminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adminName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    adminEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSlat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.adminID);
                    table.CheckConstraint("CK_Admins_Email_Gmail", "[adminEmail] LIKE '%@%.com'");
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    categoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    categoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.categoryID);
                });

            migrationBuilder.CreateTable(
                name: "jobTitles",
                columns: table => new
                {
                    JobTilteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTilteName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobTitles", x => x.JobTilteID);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSlat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserID);
                    table.CheckConstraint("CK_Users_Email_Gmail", "[UserEmail] LIKE '%@%.com'");
                });

            migrationBuilder.CreateTable(
                name: "instructors",
                columns: table => new
                {
                    instructorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    jobTitleID = table.Column<int>(type: "int", nullable: false),
                    instructorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    instructorDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    instructorImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instructors", x => x.instructorID);
                    table.ForeignKey(
                        name: "FK_instructors_jobTitles_jobTitleID",
                        column: x => x.jobTitleID,
                        principalTable: "jobTitles",
                        principalColumn: "JobTilteID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CardName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaypalEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_payments_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "userCoursesHeaders",
                columns: table => new
                {
                    UserCoursesHeaderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userCoursesHeaders", x => x.UserCoursesHeaderID);
                    table.ForeignKey(
                        name: "FK_userCoursesHeaders_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    courseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    categoryID = table.Column<int>(type: "int", nullable: false),
                    instructorID = table.Column<int>(type: "int", nullable: false),
                    courseLevel = table.Column<int>(type: "int", nullable: false),
                    courseRate = table.Column<decimal>(type: "decimal(3,3)", nullable: false),
                    courseHours = table.Column<decimal>(type: "decimal(7,3)", nullable: false),
                    CoursePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CourseDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCertification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.courseID);
                    table.CheckConstraint("CK_Course_Rate_0<x<=5", "[courseRate] > 0 AND [courseRate] <= 5");
                    table.ForeignKey(
                        name: "FK_courses_categories_categoryID",
                        column: x => x.categoryID,
                        principalTable: "categories",
                        principalColumn: "categoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_courses_instructors_instructorID",
                        column: x => x.instructorID,
                        principalTable: "instructors",
                        principalColumn: "instructorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "contents",
                columns: table => new
                {
                    contentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseID = table.Column<int>(type: "int", nullable: false),
                    contentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LecturesNumber = table.Column<int>(type: "int", nullable: false),
                    contentHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateDateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contents", x => x.contentID);
                    table.ForeignKey(
                        name: "FK_contents_courses_courseID",
                        column: x => x.courseID,
                        principalTable: "courses",
                        principalColumn: "courseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userCoursesDetails",
                columns: table => new
                {
                    UserCoursesDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCoursesHeaderID = table.Column<int>(type: "int", nullable: false),
                    courseID = table.Column<int>(type: "int", nullable: false),
                    coursePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userCoursesDetails", x => x.UserCoursesDetailID);
                    table.ForeignKey(
                        name: "FK_userCoursesDetails_courses_courseID",
                        column: x => x.courseID,
                        principalTable: "courses",
                        principalColumn: "courseID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_userCoursesDetails_userCoursesHeaders_UserCoursesHeaderID",
                        column: x => x.UserCoursesHeaderID,
                        principalTable: "userCoursesHeaders",
                        principalColumn: "UserCoursesHeaderID",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_categories_categoryName",
                table: "categories",
                column: "categoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contents_courseID",
                table: "contents",
                column: "courseID");

            migrationBuilder.CreateIndex(
                name: "IX_courses_categoryID",
                table: "courses",
                column: "categoryID");

            migrationBuilder.CreateIndex(
                name: "IX_courses_instructorID",
                table: "courses",
                column: "instructorID");

            migrationBuilder.CreateIndex(
                name: "IX_instructors_jobTitleID",
                table: "instructors",
                column: "jobTitleID");

            migrationBuilder.CreateIndex(
                name: "IX_jobTitles_JobTilteName",
                table: "jobTitles",
                column: "JobTilteName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_UserID",
                table: "payments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_userCoursesDetails_courseID",
                table: "userCoursesDetails",
                column: "courseID");

            migrationBuilder.CreateIndex(
                name: "IX_userCoursesDetails_UserCoursesHeaderID",
                table: "userCoursesDetails",
                column: "UserCoursesHeaderID");

            migrationBuilder.CreateIndex(
                name: "IX_userCoursesHeaders_UserID",
                table: "userCoursesHeaders",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_users_UserEmail",
                table: "users",
                column: "UserEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_UserName",
                table: "users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "contents");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "userCoursesDetails");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "userCoursesHeaders");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "instructors");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "jobTitles");
        }
    }
}

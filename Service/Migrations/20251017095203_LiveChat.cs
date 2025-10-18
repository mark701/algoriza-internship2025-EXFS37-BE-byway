using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.Migrations
{
    /// <inheritdoc />
    public partial class LiveChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chatMessages",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderID = table.Column<int>(type: "int", nullable: false),
                    ReceiverID = table.Column<int>(type: "int", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatMessages", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_chatMessages_users_ReceiverID",
                        column: x => x.ReceiverID,
                        principalTable: "users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_chatMessages_users_SenderID",
                        column: x => x.SenderID,
                        principalTable: "users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "userConnections",
                columns: table => new
                {
                    ConnectionID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ConnectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userConnections", x => x.ConnectionID);
                    table.ForeignKey(
                        name: "FK_userConnections_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chatMessages_ReceiverID",
                table: "chatMessages",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_chatMessages_SenderID",
                table: "chatMessages",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_userConnections_UserID",
                table: "userConnections",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chatMessages");

            migrationBuilder.DropTable(
                name: "userConnections");
        }
    }
}

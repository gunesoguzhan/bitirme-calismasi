using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meet.ProfileService.WebAPI.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendshipRequests",
                columns: table => new
                {
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendshipRequests", x => new { x.ReceiverId, x.SenderId });
                    table.ForeignKey(
                        name: "FK_FriendshipRequests_Profiles_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendshipRequests_Profiles_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    Profile1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profile2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => new { x.Profile1Id, x.Profile2Id });
                    table.ForeignKey(
                        name: "FK_Friendships_Profiles_Profile1Id",
                        column: x => x.Profile1Id,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendships_Profiles_Profile2Id",
                        column: x => x.Profile2Id,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendshipRequests_SenderId",
                table: "FriendshipRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_Profile2Id",
                table: "Friendships",
                column: "Profile2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendshipRequests");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}

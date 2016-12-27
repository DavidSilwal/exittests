using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class userhistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLoginHistories",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    UserLoginHistoryID = table.Column<string>(nullable: false),
                    Browser = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    LastActivity = table.Column<DateTime>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginHistories", x => new { x.UserID, x.UserLoginHistoryID });
                    table.ForeignKey(
                        name: "FK_UserLoginHistories_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginHistories_UserID",
                table: "UserLoginHistories",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLoginHistories");
        }
    }
}

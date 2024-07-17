using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MissedMessages",
                newName: "SenderId");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeen",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "MissedMessages",
                newName: "UserId");
        }
    }
}

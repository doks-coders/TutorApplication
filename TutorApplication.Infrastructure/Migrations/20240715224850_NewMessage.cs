using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CourseGroupId",
                table: "UserGroups",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeen",
                table: "UserGroups",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecieverId",
                table: "UserGroups",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "isGroup",
                table: "UserGroups",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseGroupId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "RecieverId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "isGroup",
                table: "UserGroups");
        }
    }
}

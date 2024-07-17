using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "Courses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PhotoId",
                table: "Courses",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Photos_PhotoId",
                table: "Courses",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Photos_PhotoId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_PhotoId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Courses");
        }
    }
}

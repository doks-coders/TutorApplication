using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_PhotoId",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PhotoId",
                table: "Courses",
                column: "PhotoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_PhotoId",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PhotoId",
                table: "Courses",
                column: "PhotoId");
        }
    }
}

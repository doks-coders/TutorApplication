using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuizId",
                table: "Messages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_QuizId",
                table: "Messages",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Quizs_QuizId",
                table: "Messages",
                column: "QuizId",
                principalTable: "Quizs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Quizs_QuizId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_QuizId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Messages");
        }
    }
}

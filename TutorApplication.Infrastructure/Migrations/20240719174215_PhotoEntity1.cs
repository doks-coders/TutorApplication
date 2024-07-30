﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PhotoEntity1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Photos_MessageId",
                table: "Photos",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Messages_MessageId",
                table: "Photos",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Messages_MessageId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_MessageId",
                table: "Photos");
        }
    }
}

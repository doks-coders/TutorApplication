using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorApplication.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class Migration11 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Username",
				table: "Connections",
				type: "text",
				nullable: false,
				defaultValue: "");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Username",
				table: "Connections");
		}
	}
}

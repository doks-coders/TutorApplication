using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TutorApplication.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class Migration1 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AspNetRoles",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUsers",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					FirstName = table.Column<string>(type: "text", nullable: true),
					LastName = table.Column<string>(type: "text", nullable: true),
					FullName = table.Column<string>(type: "text", nullable: true),
					FullNameBackwards = table.Column<string>(type: "text", nullable: true),
					Title = table.Column<string>(type: "text", nullable: true),
					About = table.Column<string>(type: "text", nullable: true),
					DateUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					NavigationId = table.Column<Guid>(type: "uuid", nullable: false),
					AccountType = table.Column<string>(type: "text", nullable: false),
					isProfileUpdated = table.Column<bool>(type: "boolean", nullable: false),
					isAccountValidated = table.Column<bool>(type: "boolean", nullable: false),
					LockAccount = table.Column<bool>(type: "boolean", nullable: false),
					UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
					PasswordHash = table.Column<string>(type: "text", nullable: true),
					SecurityStamp = table.Column<string>(type: "text", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
					PhoneNumber = table.Column<string>(type: "text", nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
					LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
					AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUsers", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Groups",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Name = table.Column<string>(type: "text", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Groups", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AspNetRoleClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					RoleId = table.Column<Guid>(type: "uuid", nullable: false),
					ClaimType = table.Column<string>(type: "text", nullable: true),
					ClaimValue = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserClaims",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					ClaimType = table.Column<string>(type: "text", nullable: true),
					ClaimValue = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AspNetUserClaims_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserLogins",
				columns: table => new
				{
					LoginProvider = table.Column<string>(type: "text", nullable: false),
					ProviderKey = table.Column<string>(type: "text", nullable: false),
					ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
					UserId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
					table.ForeignKey(
						name: "FK_AspNetUserLogins_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserRoles",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					RoleId = table.Column<Guid>(type: "uuid", nullable: false),
					AppUserId = table.Column<Guid>(type: "uuid", nullable: false),
					AppRoleId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetRoles_AppRoleId",
						column: x => x.AppRoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
						column: x => x.RoleId,
						principalTable: "AspNetRoles",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetUsers_AppUserId",
						column: x => x.AppUserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AspNetUserRoles_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "AspNetUserTokens",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					LoginProvider = table.Column<string>(type: "text", nullable: false),
					Name = table.Column<string>(type: "text", nullable: false),
					Value = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
					table.ForeignKey(
						name: "FK_AspNetUserTokens_AspNetUsers_UserId",
						column: x => x.UserId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Course",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					CourseTitle = table.Column<string>(type: "text", nullable: false),
					About = table.Column<string>(type: "text", nullable: false),
					Price = table.Column<int>(type: "integer", nullable: false),
					Currency = table.Column<string>(type: "text", nullable: false),
					Memos = table.Column<string>(type: "text", nullable: false),
					TutorId = table.Column<Guid>(type: "uuid", nullable: false),
					NavigationId = table.Column<Guid>(type: "uuid", nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Course", x => x.Id);
					table.ForeignKey(
						name: "FK_Course_AspNetUsers_TutorId",
						column: x => x.TutorId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Messages",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Content = table.Column<string>(type: "text", nullable: false),
					SenderId = table.Column<Guid>(type: "uuid", nullable: true),
					RecieverId = table.Column<Guid>(type: "uuid", nullable: true),
					isCourseGroup = table.Column<bool>(type: "boolean", nullable: false),
					CourseId = table.Column<Guid>(type: "uuid", nullable: false),
					isDeleted = table.Column<bool>(type: "boolean", nullable: false),
					NavigationId = table.Column<Guid>(type: "uuid", nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Messages", x => x.Id);
					table.ForeignKey(
						name: "FK_Messages_AspNetUsers_RecieverId",
						column: x => x.RecieverId,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Messages_AspNetUsers_SenderId",
						column: x => x.SenderId,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "Connections",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					GroupId = table.Column<Guid>(type: "uuid", nullable: false),
					ConnectionURL = table.Column<string>(type: "text", nullable: false),
					GroupName = table.Column<string>(type: "text", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Connections", x => x.Id);
					table.ForeignKey(
						name: "FK_Connections_Groups_GroupId",
						column: x => x.GroupId,
						principalTable: "Groups",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "CourseStudents",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					CourseId = table.Column<Guid>(type: "uuid", nullable: false),
					StudentId = table.Column<Guid>(type: "uuid", nullable: false),
					TutorId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CourseStudents", x => x.Id);
					table.ForeignKey(
						name: "FK_CourseStudents_AspNetUsers_StudentId",
						column: x => x.StudentId,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_CourseStudents_AspNetUsers_TutorId",
						column: x => x.TutorId,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_CourseStudents_Course_CourseId",
						column: x => x.CourseId,
						principalTable: "Course",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_AspNetRoleClaims_RoleId",
				table: "AspNetRoleClaims",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				table: "AspNetRoles",
				column: "NormalizedName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserClaims_UserId",
				table: "AspNetUserClaims",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserLogins_UserId",
				table: "AspNetUserLogins",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_AppRoleId",
				table: "AspNetUserRoles",
				column: "AppRoleId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_AppUserId",
				table: "AspNetUserRoles",
				column: "AppUserId");

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUserRoles_RoleId",
				table: "AspNetUserRoles",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				table: "AspNetUsers",
				column: "NormalizedEmail");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				table: "AspNetUsers",
				column: "NormalizedUserName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Connections_GroupId",
				table: "Connections",
				column: "GroupId");

			migrationBuilder.CreateIndex(
				name: "IX_Course_TutorId",
				table: "Course",
				column: "TutorId");

			migrationBuilder.CreateIndex(
				name: "IX_CourseStudents_CourseId",
				table: "CourseStudents",
				column: "CourseId");

			migrationBuilder.CreateIndex(
				name: "IX_CourseStudents_StudentId",
				table: "CourseStudents",
				column: "StudentId");

			migrationBuilder.CreateIndex(
				name: "IX_CourseStudents_TutorId",
				table: "CourseStudents",
				column: "TutorId");

			migrationBuilder.CreateIndex(
				name: "IX_Messages_RecieverId",
				table: "Messages",
				column: "RecieverId");

			migrationBuilder.CreateIndex(
				name: "IX_Messages_SenderId",
				table: "Messages",
				column: "SenderId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AspNetRoleClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserClaims");

			migrationBuilder.DropTable(
				name: "AspNetUserLogins");

			migrationBuilder.DropTable(
				name: "AspNetUserRoles");

			migrationBuilder.DropTable(
				name: "AspNetUserTokens");

			migrationBuilder.DropTable(
				name: "Connections");

			migrationBuilder.DropTable(
				name: "CourseStudents");

			migrationBuilder.DropTable(
				name: "Messages");

			migrationBuilder.DropTable(
				name: "AspNetRoles");

			migrationBuilder.DropTable(
				name: "Groups");

			migrationBuilder.DropTable(
				name: "Course");

			migrationBuilder.DropTable(
				name: "AspNetUsers");
		}
	}
}

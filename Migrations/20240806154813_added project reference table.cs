using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectOnBoarding.Migrations
{
    /// <inheritdoc />
    public partial class addedprojectreferencetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "dbo",
                newName: "UserTokens");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "dbo",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "dbo",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "dbo",
                newName: "UserLogins");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "dbo",
                newName: "UserClaims");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                schema: "dbo",
                newName: "RoleClaims");

            migrationBuilder.RenameTable(
                name: "Role",
                schema: "dbo",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "dbo",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "Divisions",
                schema: "dbo",
                newName: "Divisions");

            migrationBuilder.RenameTable(
                name: "Companies",
                schema: "dbo",
                newName: "Companies");

            migrationBuilder.RenameTable(
                name: "Brands",
                schema: "dbo",
                newName: "Brands");

            migrationBuilder.CreateTable(
                name: "ProjectReferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    File = table.Column<string>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectReferences_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReferences_ProjectId",
                table: "ProjectReferences",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectReferences");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "UserTokens",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                newName: "UserLogins",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "UserClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                newName: "RoleClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Role",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Projects",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Divisions",
                newName: "Divisions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Companies",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brands",
                newSchema: "dbo");
        }
    }
}

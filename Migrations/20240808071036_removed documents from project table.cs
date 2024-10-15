using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectOnBoarding.Migrations
{
    /// <inheritdoc />
    public partial class removeddocumentsfromprojecttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BriefFile",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReferenceFile",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BriefFile",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceFile",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }
    }
}

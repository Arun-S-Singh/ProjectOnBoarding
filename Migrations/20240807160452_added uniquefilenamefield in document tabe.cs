using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectOnBoarding.Migrations
{
    /// <inheritdoc />
    public partial class addeduniquefilenamefieldindocumenttabe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueFileName",
                table: "Documents",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueFileName",
                table: "Documents");
        }
    }
}

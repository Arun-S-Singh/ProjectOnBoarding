using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectOnBoarding.Migrations
{
    /// <inheritdoc />
    public partial class removedsizeandcategoryfromprojectandaddediscompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Projects");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }
    }
}

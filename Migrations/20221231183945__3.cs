using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorHomeSite.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayTimeSeconds",
                table: "Songs");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Songs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Songs");

            migrationBuilder.AddColumn<double>(
                name: "PlayTimeSeconds",
                table: "Songs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

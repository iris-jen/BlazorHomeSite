using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorHomeSite.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailPath",
                table: "Photos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPath",
                table: "Photos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}

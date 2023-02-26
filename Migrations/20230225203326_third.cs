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
            migrationBuilder.AddColumn<bool>(
                name: "EnableBlogFeature",
                table: "SiteOwners",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableMusicFeature",
                table: "SiteOwners",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePhotosFeature",
                table: "SiteOwners",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PhotoAlbumId",
                table: "PhotoTags",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhotoTags_PhotoAlbumId",
                table: "PhotoTags",
                column: "PhotoAlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoTags_PhotoAlbums_PhotoAlbumId",
                table: "PhotoTags",
                column: "PhotoAlbumId",
                principalTable: "PhotoAlbums",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoTags_PhotoAlbums_PhotoAlbumId",
                table: "PhotoTags");

            migrationBuilder.DropIndex(
                name: "IX_PhotoTags_PhotoAlbumId",
                table: "PhotoTags");

            migrationBuilder.DropColumn(
                name: "EnableBlogFeature",
                table: "SiteOwners");

            migrationBuilder.DropColumn(
                name: "EnableMusicFeature",
                table: "SiteOwners");

            migrationBuilder.DropColumn(
                name: "EnablePhotosFeature",
                table: "SiteOwners");

            migrationBuilder.DropColumn(
                name: "PhotoAlbumId",
                table: "PhotoTags");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorHomeSite.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicAlbums_Photos_AlbumCoverId",
                table: "MusicAlbums");

            migrationBuilder.DropIndex(
                name: "IX_MusicAlbums_AlbumCoverId",
                table: "MusicAlbums");

            migrationBuilder.DropColumn(
                name: "AlbumCoverId",
                table: "MusicAlbums");

            migrationBuilder.AddColumn<int>(
                name: "AlbumOrder",
                table: "PhotoAlbums",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AlbumCoverPhotoId",
                table: "MusicAlbums",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlbumOrder",
                table: "PhotoAlbums");

            migrationBuilder.DropColumn(
                name: "AlbumCoverPhotoId",
                table: "MusicAlbums");

            migrationBuilder.AddColumn<int>(
                name: "AlbumCoverId",
                table: "MusicAlbums",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MusicAlbums_AlbumCoverId",
                table: "MusicAlbums",
                column: "AlbumCoverId");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicAlbums_Photos_AlbumCoverId",
                table: "MusicAlbums",
                column: "AlbumCoverId",
                principalTable: "Photos",
                principalColumn: "Id");
        }
    }
}

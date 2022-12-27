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
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PhotoAlbums_AlbumId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "PhotoAlbumPhotoAlbumTags");

            migrationBuilder.DropTable(
                name: "PhotoAlbumTags");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoPath",
                table: "Photos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "Photos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<bool>(
                name: "IsAlbumCover",
                table: "Photos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LocationCoOrdinates",
                table: "Photos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PhotoTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhotoPhotoTags",
                columns: table => new
                {
                    PhotosId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoPhotoTags", x => new { x.PhotosId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PhotoPhotoTags_PhotoTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "PhotoTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoPhotoTags_Photos_PhotosId",
                        column: x => x.PhotosId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoPhotoTags_TagsId",
                table: "PhotoPhotoTags",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PhotoAlbums_AlbumId",
                table: "Photos",
                column: "AlbumId",
                principalTable: "PhotoAlbums",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PhotoAlbums_AlbumId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "PhotoPhotoTags");

            migrationBuilder.DropTable(
                name: "PhotoTags");

            migrationBuilder.DropColumn(
                name: "IsAlbumCover",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "LocationCoOrdinates",
                table: "Photos");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoPath",
                table: "Photos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "Photos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PhotoAlbumTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoAlbumTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhotoAlbumPhotoAlbumTags",
                columns: table => new
                {
                    AlbumTagsId = table.Column<int>(type: "INTEGER", nullable: false),
                    AlbumsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoAlbumPhotoAlbumTags", x => new { x.AlbumTagsId, x.AlbumsId });
                    table.ForeignKey(
                        name: "FK_PhotoAlbumPhotoAlbumTags_PhotoAlbumTags_AlbumTagsId",
                        column: x => x.AlbumTagsId,
                        principalTable: "PhotoAlbumTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoAlbumPhotoAlbumTags_PhotoAlbums_AlbumsId",
                        column: x => x.AlbumsId,
                        principalTable: "PhotoAlbums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoAlbumPhotoAlbumTags_AlbumsId",
                table: "PhotoAlbumPhotoAlbumTags",
                column: "AlbumsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PhotoAlbums_AlbumId",
                table: "Photos",
                column: "AlbumId",
                principalTable: "PhotoAlbums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

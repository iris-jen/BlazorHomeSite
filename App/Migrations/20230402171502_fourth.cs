using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorHomeSite.Migrations
{
    /// <inheritdoc />
    public partial class fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteOwners_Photos_HomePageBackgroundId",
                table: "SiteOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_SiteOwners_Photos_ProfilePhotoId",
                table: "SiteOwners");

            migrationBuilder.DropIndex(
                name: "IX_SiteOwners_HomePageBackgroundId",
                table: "SiteOwners");

            migrationBuilder.DropIndex(
                name: "IX_SiteOwners_ProfilePhotoId",
                table: "SiteOwners");

            migrationBuilder.DeleteData(
                table: "SiteOwners",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "EnableBlogFeature",
                table: "SiteOwners");

            migrationBuilder.DropColumn(
                name: "HomePageBackgroundId",
                table: "SiteOwners");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoId",
                table: "SiteOwners");

            migrationBuilder.RenameColumn(
                name: "EnablePhotosFeature",
                table: "SiteOwners",
                newName: "PhotoIdProfile");

            migrationBuilder.RenameColumn(
                name: "EnableMusicFeature",
                table: "SiteOwners",
                newName: "PhotoIdHomePageBackground");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoIdProfile",
                table: "SiteOwners",
                newName: "EnablePhotosFeature");

            migrationBuilder.RenameColumn(
                name: "PhotoIdHomePageBackground",
                table: "SiteOwners",
                newName: "EnableMusicFeature");

            migrationBuilder.AddColumn<bool>(
                name: "EnableBlogFeature",
                table: "SiteOwners",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "HomePageBackgroundId",
                table: "SiteOwners",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfilePhotoId",
                table: "SiteOwners",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.InsertData(
                table: "SiteOwners",
                columns: new[] { "Id", "About", "DiscordUrl", "EnableBlogFeature", "EnableMusicFeature", "EnablePhotosFeature", "FirstName", "GitHubUrl", "HomePageBackgroundId", "Introduction", "LastName", "LinkdinUrl", "Location", "ProfilePhotoId" },
                values: new object[] { 1, null, null, false, false, false, null, null, null, null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_SiteOwners_HomePageBackgroundId",
                table: "SiteOwners",
                column: "HomePageBackgroundId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteOwners_ProfilePhotoId",
                table: "SiteOwners",
                column: "ProfilePhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteOwners_Photos_HomePageBackgroundId",
                table: "SiteOwners",
                column: "HomePageBackgroundId",
                principalTable: "Photos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteOwners_Photos_ProfilePhotoId",
                table: "SiteOwners",
                column: "ProfilePhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }
    }
}

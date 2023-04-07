using BlazorHomeSite.Data.Comments;
using BlazorHomeSite.Data.Music;
using BlazorHomeSite.Data.Photos;
using BlazorHomeSite.Data.SiteOwners;
using BlazorHomeSite.Data.Tags;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Services.Database
{
    public interface IDatabaseService
    {
        DbSet<Comment> Comments { get; }
        DbSet<MusicAlbum> MusicAlbums { get; }
        DbSet<PhotoAlbum> PhotoAlbums { get; }
        DbSet<Photo> Photos { get; }
        DbSet<SiteOwner> SiteOwners { get; }
        DbSet<Song> Songs { get; }
        DbSet<Tag> Tags { get; }

        Task<bool> CreateDbAsync();

        Task<bool> DeleteDbAsync();

        public void SaveDb();

        public Task SaveDbAsync();
    }
}
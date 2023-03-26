using BlazorHomeSite.Data.Domain;
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
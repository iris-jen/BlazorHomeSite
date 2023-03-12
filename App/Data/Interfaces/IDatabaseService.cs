using BlazorHomeSite.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Data.Interfaces
{
    public interface IDatabaseService
    {
        DbSet<Comment> Comments { get; }
        DbSet<MusicAlbum> MusicAlbums { get; }
        DbSet<PhotoAlbum> PhotoAlbums { get; }
        DbSet<Photo> Photos { get; }
        DbSet<Song> Songs { get; }
        DbSet<Tag> Tags { get; }

        Task<bool> EnsureCreatedAsync();

        Task<bool> EnsureDeletedAsync();

        Task<SiteOwner?> GetSiteOwner();

        public Task SaveChangesAsync();
    }
}
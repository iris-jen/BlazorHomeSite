using BlazorHomeSite.Data.Comments;
using BlazorHomeSite.Data.Music;
using BlazorHomeSite.Data.Photos;
using BlazorHomeSite.Data.SiteOwners;
using BlazorHomeSite.Data.Tags;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Services.Database;

public class DatabaseService : IdentityDbContext, IDatabaseService
{
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<MusicAlbum> MusicAlbums => Set<MusicAlbum>();

    public DbSet<PhotoAlbum> PhotoAlbums => Set<PhotoAlbum>();

    public DbSet<Photo> Photos => Set<Photo>();

    public DbSet<SiteOwner> SiteOwners => Set<SiteOwner>();

    public DbSet<Song> Songs => Set<Song>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DatabaseService(DbContextOptions<DatabaseService> options) : base(options)
    {
    }

    public async Task<bool> CreateDbAsync()
    {
        return await Database.EnsureCreatedAsync();
    }

    public async Task<bool> DeleteDbAsync()
    {
        return await Database.EnsureDeletedAsync();
    }

    public void SaveDb()
    {
        SaveChanges();
    }

    public async Task SaveDbAsync()
    {
        await SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<PhotoAlbum>()
            .HasKey(x => x.Id);

        mb.Entity<Tag>()
            .HasKey(x => x.Id);

        mb.Entity<Comment>()
            .HasKey(x => x.Id);

        mb.Entity<Photo>()
            .HasKey(x => x.Id);

        mb.Entity<Photo>()
            .HasIndex(x => new { x.AlbumId, x.CaptureTime });

        mb.Entity<Photo>()
            .HasOne(x => x.Album)
            .WithMany(x => x.Photos);

        mb.Entity<MusicAlbum>().HasKey(x => x.Id);
        mb.Entity<Song>().HasOne(x => x.Album).WithMany(x => x.Songs);

        mb.Entity<SiteOwner>().HasKey(x => x.Id);
    }
}
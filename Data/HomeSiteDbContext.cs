using BlazorHomeSite.Data.Music;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Data;

public class HomeSiteDbContext : IdentityDbContext
{
    public HomeSiteDbContext(DbContextOptions<HomeSiteDbContext> options)
        : base(options)
    {
    }

    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<PhotoAlbum> PhotoAlbums => Set<PhotoAlbum>();
    public DbSet<PhotoTags> PhotoTags => Set<PhotoTags>();
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Song> Songs => Set<Song>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<PhotoAlbum>()
            .HasKey(x => x.Id);

        mb.Entity<PhotoTags>()
            .HasKey(x => x.Id);

        mb.Entity<PhotoTags>()
            .HasMany(x => x.Photos)
            .WithMany(x => x.Tags);

        mb.Entity<Photo>()
            .HasKey(x => x.Id);

        mb.Entity<Photo>()
            .HasIndex(x => new { x.AlbumId, x.CaptureTime });

        mb.Entity<Photo>()
            .HasOne(x => x.Album)
            .WithMany(x => x.Photos);

        mb.Entity<Album>().HasKey(x => x.Id);

        mb.Entity<Song>().HasOne(x => x.Album).WithMany(x => x.Songs);
    }
}
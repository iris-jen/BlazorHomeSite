using Domain;
using Domain.Music;
using Domain.Photos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

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
    public DbSet<SiteOwner> SiteOwners => Set<SiteOwner>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var dbName = "home-site.db";
        optionsBuilder.UseSqlite($"DataSource={dbName}; Cache=Shared");
    }

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

        mb.Entity<SiteOwner>().HasKey(x => x.ID);
        mb.Entity<SiteOwner>().HasOne(x => x.ProfilePhoto);
        mb.Entity<SiteOwner>().HasOne(x => x.HomePageBackground);
    }
}
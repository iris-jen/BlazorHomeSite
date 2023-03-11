using BlazorHomeSite.Data.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Data;

public class HomeSiteDbContext : IdentityDbContext
{
    public DbSet<Album> Albums => Set<Album>();

    public DbSet<PhotoAlbum> PhotoAlbums => Set<PhotoAlbum>();

    public DbSet<Photo> Photos => Set<Photo>();

    public DbSet<SiteOwner> SiteOwners => Set<SiteOwner>();

    public DbSet<Song> Songs => Set<Song>();

    public DbSet<Tag> Tags => Set<Tag>();

    public HomeSiteDbContext(DbContextOptions<HomeSiteDbContext> options)
                                : base(options)
    {
    }

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

        mb.Entity<Tag>()
            .HasKey(x => x.Id);

        mb.Entity<Photo>()
            .HasKey(x => x.Id);

        mb.Entity<Photo>()
            .HasIndex(x => new { x.AlbumId, x.CaptureTime });

        mb.Entity<Photo>()
            .HasOne(x => x.Album)
            .WithMany(x => x.Photos);

        mb.Entity<Album>().HasKey(x => x.Id);
        mb.Entity<Song>().HasOne(x => x.Album).WithMany(x => x.Songs);

        mb.Entity<SiteOwner>().HasKey(x => x.Id);
        mb.Entity<SiteOwner>().HasOne(x => x.ProfilePhoto);
        mb.Entity<SiteOwner>().HasOne(x => x.HomePageBackground);
    }
}
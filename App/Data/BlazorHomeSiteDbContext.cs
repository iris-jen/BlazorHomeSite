using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Data.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Data;

public class HomeSiteDbContext : IdentityDbContext, IDatabaseService
{
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<MusicAlbum> MusicAlbums => Set<MusicAlbum>();

    public DbSet<PhotoAlbum> PhotoAlbums => Set<PhotoAlbum>();

    public DbSet<Photo> Photos => Set<Photo>();

    public DbSet<SiteOwner> SiteOwners => Set<SiteOwner>();

    public DbSet<Song> Songs => Set<Song>();

    public DbSet<Tag> Tags => Set<Tag>();

    public HomeSiteDbContext(DbContextOptions<HomeSiteDbContext> options)
                                : base(options)
    {
    }

    public async Task<bool> EnsureCreatedAsync() => await Database.EnsureCreatedAsync();

    public async Task<bool> EnsureDeletedAsync() => await Database.EnsureDeletedAsync();

    public async Task<SiteOwner?> GetSiteOwner() => await SiteOwners.FirstOrDefaultAsync();

    public async Task SaveChangesAsync() => await SaveChangesAsync();

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
        mb.Entity<SiteOwner>().HasOne(x => x.ProfilePhoto);
        mb.Entity<SiteOwner>().HasOne(x => x.HomePageBackground);
        mb.Entity<SiteOwner>().HasData(new SiteOwner() { Id = 1 });
    }
}
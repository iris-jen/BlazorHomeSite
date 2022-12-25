using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<PhotoAlbum> PhotoAlbums => Set<PhotoAlbum>();
    public DbSet<PhotoAlbumTags> PhotoAlbumTags => Set<PhotoAlbumTags>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<PhotoAlbum>().HasKey(x => x.Id);

        mb.Entity<PhotoAlbumTags>().HasKey(x => x.Id);

        mb.Entity<PhotoAlbumTags>()
            .HasMany(x => x.Albums)
            .WithMany(x => x.AlbumTags);

        mb.Entity<Photo>().HasKey(x => x.Id);
        mb.Entity<Photo>().HasOne(x => x.Album)
            .WithMany(x => x.Photos);
    }
}
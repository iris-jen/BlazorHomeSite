using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Photo> Photos => Set<Photo>();
    public DbSet<PhotoAlbum> PhotoAlbums => Set<PhotoAlbum>();
    public DbSet<PhotoTags> PhotoTags => Set<PhotoTags>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
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
            .HasIndex(x => new {x.AlbumId, x.CaptureTime} );

        mb.Entity<Photo>()
            .HasOne(x => x.Album)
            .WithMany(x => x.Photos);
    }
}
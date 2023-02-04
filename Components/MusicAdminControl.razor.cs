using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Music;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Components;

public partial class MusicAdminControl
{
    [Inject] private IDbContextFactory<HomeSiteDbContext> DbFactory { get; set; } = null!;
    [Inject] private IWebHostEnvironment HostEnvironment { get; set; } = null!;
    [Inject] private ILogger<PhotoAlbumAdminControl> Logger { get; set; } = null!;

    public async Task InitalizeAlbumsFromRoot()
    {
        await using var context = await DbFactory?.CreateDbContextAsync()!;
        var musicRoot = new DirectoryInfo(Path.Combine(HostEnvironment.WebRootPath, "music"));

        foreach (var dir in musicRoot.EnumerateDirectories())
        {
            var album = new Album
            {
                AlbumName = dir.Name,
                Description = "",
                Songs = new List<Song>()
            };

            foreach (var file in dir.EnumerateFiles())
                if (file.Name.EndsWith(".mp3"))
                {
                    var song = new Song
                    {
                        SongName = file.Name,
                        Path = Path.Combine("music", dir.Name, file.Name),
                        Lyrics = "",
                        Format = file.Name.Split('.')[1]
                    };
                    album.Songs.Add(song);
                }
                else if (file.Name.EndsWith(".jpg"))
                {
                    album.AlbumCover = Path.Combine("music", dir.Name, file.Name);
                }

            await context.Albums.AddAsync(album);
            await context.SaveChangesAsync();
        }
    }

    public async Task ClearAlbums()
    {
        await using var context = await DbFactory?.CreateDbContextAsync()!;
        foreach (var album in context.Albums.Include(x => x.Songs))
        {
            context.Songs.RemoveRange(album.Songs);
            context.Albums.Remove(album);
        }

        await context.SaveChangesAsync();
    }
}
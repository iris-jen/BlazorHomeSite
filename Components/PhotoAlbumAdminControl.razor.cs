using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Components;

public partial class PhotoAlbumAdminControl
{
    [Inject]
    private IDbContextFactory<ApplicationDbContext>? DbFactory { get; set; }

    [Inject]
    private IWebHostEnvironment HostEnviroment { get; set; }
    
    private async Task AddWebRootPhotosToDb()
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        var root = HostEnviroment.WebRootPath;
        var photos = Path.Combine(root, "photos");
        var subDirectories = new DirectoryInfo(photos);

        foreach (var directory in subDirectories.EnumerateDirectories())
        {
            var album = CreatePhotoAlbumFromDir(directory);
            await context.PhotoAlbums.AddAsync(album);
        }

        await context.SaveChangesAsync();
    }

    private PhotoAlbum CreatePhotoAlbumFromDir(DirectoryInfo directory)
    {
        var album = new PhotoAlbum()
        {
            Description = directory.Name,
            Photos = new List<Photo>()
        };

        var isFirst = true;
        foreach (var file in directory.EnumerateFiles())
        {
            var photo = new Photo()
            {
                PhotoPath = file.FullName,
                CaptureTime = file.CreationTime,
                IsAlbumCover = isFirst
            };
            album.Photos.Add(photo);
            isFirst = false;
        }

        return album;
    }

    private async Task ClearAllAlbums()
    {
        await using var context = await DbFactory.CreateDbContextAsync();

        foreach (var album in context.PhotoAlbums)
        {
            context.PhotoAlbums.Remove(album);
        }
        await context.SaveChangesAsync();
    }
       
}
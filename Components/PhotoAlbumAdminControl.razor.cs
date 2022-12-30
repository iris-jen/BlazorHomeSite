using System.Collections.Concurrent;
using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Components;

public partial class PhotoAlbumAdminControl
{
    private const string MegaAlbumDescription = "All The Rest";
    [Inject] private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; } = null!;
    [Inject] private IWebHostEnvironment HostEnvironment { get; set; } = null!;
    [Inject] private ILogger<PhotosAdminControl> Logger { get; set; } = null!;

    private bool Working { get; set; }

    private async Task AddWebRootPhotosToDb()
    {
        try
        {
            Logger.LogInformation("Adding photos to db... ");
            var megaAlbumId = -1;
            await using (var context = await DbFactory?.CreateDbContextAsync()!)
            {
                megaAlbumId = await GetOrCreateMegaAlbum(context);
            }

            var root = HostEnvironment.WebRootPath;
            var photos = Path.Combine(root, "photos");
            var subDirectories = new DirectoryInfo(photos);

            foreach (var dir in subDirectories.EnumerateDirectories().Where(x => !x.Name.Equals("thumbs")))
            {
                Logger.LogInformation($"Processing {dir.Name}");
                if (dir.Name.StartsWith("all_"))
                {
                    await SavePhotos(dir, megaAlbumId);
                }
                else
                {
                    var tempAlbum = new PhotoAlbum
                    {
                        Description = dir.Name
                    };

                    await using (var context = await DbFactory?.CreateDbContextAsync()!)
                    {
                        context.PhotoAlbums.Add(tempAlbum);
                        await context.SaveChangesAsync();
                    }

                    await SavePhotos(dir, tempAlbum.Id);
                }

                Logger.LogInformation($@"Finished Processing {dir.Name}");
            }

            ;
            Logger.LogInformation("All Photos Processed");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Could not process photos");
        }
    }

    private static async Task<int> GetOrCreateMegaAlbum(ApplicationDbContext context)
    {
        var megaAlbum = await context.PhotoAlbums
            .FirstOrDefaultAsync(x => x.Description.Equals(MegaAlbumDescription));

        if (megaAlbum != null) return -1;

        var allPhotos = new PhotoAlbum
        {
            Description = MegaAlbumDescription
        };
        context.PhotoAlbums.Add(allPhotos);
        await context.SaveChangesAsync();
        return allPhotos.Id;
    }

    private async Task SavePhotos(DirectoryInfo directory, int albumId)
    {
        var bag = new ConcurrentBag<Photo>();
        Parallel.ForEach(directory.EnumerateFiles(), file =>
        {
            var dateTaken = DataHelper.ShrinkImage(file.FullName,
                Path.Combine(HostEnvironment.WebRootPath, "photos", "thumbs", file.Name),
                150,
                150);

            bag.Add(new Photo
            {
                PhotoPath = Path.Combine("photos", directory.Name, file.Name),
                ThumbnailPath = Path.Combine("photos", "thumbs", file.Name),
                CaptureTime = dateTaken,
                AlbumId = albumId
            });
        });

        await using var context = await DbFactory?.CreateDbContextAsync()!;
        await context.Photos.AddRangeAsync(bag.ToList());
        await context.SaveChangesAsync();
    }

    private void ClearAllAlbums()
    {
        Working = true;
        if (DbFactory != null)
        {
            using var context = DbFactory.CreateDbContext();
            var data =
                context.PhotoAlbums.Include(x => x.Photos).ToArray();

            for (var i = 0; i < data.Length; i++)
            {
                var photos = data[i].Photos;
                if (photos != null)
                    context.Photos.RemoveRange(photos);

                context.PhotoAlbums.Remove(data[i]);
            }

            context.SaveChanges();
            Working = false;
            StateHasChanged();
        }
    }
}
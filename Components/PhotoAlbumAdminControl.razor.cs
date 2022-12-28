using System.Collections.Concurrent;
using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Nextended.Core.Extensions;

namespace BlazorHomeSite.Components;

public partial class PhotoAlbumAdminControl
{
    [Inject] private IDbContextFactory<ApplicationDbContext> DbFactory { get; set; } = null!;
    [Inject] private IWebHostEnvironment HostEnvironment { get; set; } = null!;
    [Inject] private ILogger<PhotosAdminControl> Logger { get; set; } = null!;

    private bool Working { get; set; }
    private int ProgressBarCurrent { get; set; }
    private int ProgressBarMax { get; set; }
    private int ProgressBarMin { get; set; }
    private const string MegaAlbumDescription = "All The Rest";

    private async Task AddWebRootPhotosToDb()
    {
        try
        {
            Logger.LogInformation("Adding photos to db... ");

            await using var context = await DbFactory?.CreateDbContextAsync()!;
            var megaAlbumId = await GetOrCreateMegaAlbum(context);

            var root = HostEnvironment.WebRootPath;
            var photos = Path.Combine(root, "photos");
            var subDirectories = new DirectoryInfo(photos);
            
            foreach(var dir in subDirectories.EnumerateDirectories())
            {
                Logger.LogInformation($"Processing {dir.Name}");
                if (dir.Name.StartsWith("all_"))
                {
                    var megaAlbum = await context.PhotoAlbums.Include(x=>x.Photos)
                                                              .AsSplitQuery()
                                                              .FirstOrDefaultAsync(x => x.Id == megaAlbumId);


                    megaAlbum.Photos.AddRange(GetPhotosForAlbum(dir));
                    context.PhotoAlbums.Update(megaAlbum);
                    await context.SaveChangesAsync();
                }
                else
                {
                    context.PhotoAlbums.Add(new PhotoAlbum
                    {
                        Description = dir.Name,
                        Photos = GetPhotosForAlbum(dir)
                    });
                    
                    await context.SaveChangesAsync();
                }

                Logger.LogInformation($"Finished Processing {dir.Name}");
            };
            
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
            Description = MegaAlbumDescription,
        };
        context.PhotoAlbums.Add(allPhotos);
        await context.SaveChangesAsync();
        return allPhotos.Id;
    }
    

    private List<Photo> GetPhotosForAlbum(DirectoryInfo directory)
    {
        var photoBag = new ConcurrentBag<Photo>();

        Parallel.ForEach(directory.EnumerateFiles(), new ParallelOptions(){MaxDegreeOfParallelism = 2}, file =>
        {
            {
                var path = Path.Combine("photos", directory.Name, file.Name);
                var thumbPath = Path.Combine("wwwroot", "photos", "thumbs");

                var thumbGuid = Guid.NewGuid() + ".jpg";
                var hostPath = HostEnvironment.WebRootPath;
                var thumbPathFull = Path.Combine(hostPath, "photos", "thumbs", thumbGuid);

                DataHelper.ShrinkImage(file, thumbPathFull, 150, 150);

                var photo = new Photo
                {
                    PhotoPath = path,
                    ThumbnailPath = Path.Combine(thumbPath, thumbGuid),
                    CaptureTime = file.CreationTime,
                };
                photoBag.Add(photo);

            }
        });
        
        return photoBag.ToList();
    }

    private void ClearAllAlbums()
    {
        Working = true;
        if (DbFactory != null)
        {
            using var context = DbFactory.CreateDbContext();
            
            var data =
                context.PhotoAlbums.Include(x => x.Photos).ToArray();
            
            ProgressBarCurrent = 0;
            ProgressBarMin = 0;
            ProgressBarMax = data.Length;
            StateHasChanged();
            
            for (var i = 0; i < data.Length; i++)
            {
                var photos = data[i].Photos;
                if (photos != null)
                    context.Photos.RemoveRange(photos);
                
                context.PhotoAlbums.Remove(data[i]);
                ProgressBarCurrent = i;
            }

            context.SaveChanges();
            Working = false;
            StateHasChanged();
        }
    }
}
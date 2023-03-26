using Ardalis.Result;
using Ardalis.Specification;
using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using BlazorHomeSite.Services.Database;
using Newtonsoft.Json;
using SixLabors.ImageSharp.Formats;

namespace BlazorHomeSite.Services.Photos;

public class UploadPhotosService : IUploadPhotoService
{
    private readonly IDatabaseService _datbaseService;
    private readonly ILogger<UploadPhotosService> _logger;
    private readonly string _photoBaseDir;

    public UploadPhotosService(IDatabaseService databaseService,
                               ILogger<UploadPhotosService> logger,
                               IOptions<AppAdminOptions> adminOptions)
    {
        _datbaseService = databaseService;
        _logger = logger;
        _photoBaseDir = adminOptions.Value.PhotoDirectory ??
                        Path.Combine(Environment.CurrentDirectory, "PhotoUploads");
    }

    public async Task<Result> UploadPhotos(IReadOnlyList<IBrowserFile> photos, int albumId, int maxSizeBytes = 9999999)
    {
        try
        {
            var targetDirectory = Path.Combine(_photoBaseDir, albumId.ToString());
            Directory.CreateDirectory(targetDirectory);

            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 2
            };

            var photosToSave = new ConcurrentBag<Photo>();

            foreach (var photo in photos)
            {
                try
                {
                    var targetPath = Path.Combine(targetDirectory, $"{photo.Name}");
                    await using FileStream fs = new(targetPath, FileMode.Create);
                    await photo.OpenReadStream(maxSizeBytes).CopyToAsync(fs);
                    fs.Close();

                    var image = await Image.LoadAsync(targetPath);
                    var initialExtension = targetPath.Split('.').Last();
                    var webpPath = targetPath.Replace($".{initialExtension}", ".webp");
                    await image.SaveAsWebpAsync(webpPath);
                    image.Dispose();
                    File.Delete(targetPath);

                    var webpImage = await Image.LoadAsync(webpPath);
                    var format = await Image.DetectFormatAsync(webpPath);

                    webpImage.Mutate(x => x.Resize((int)(image.Width * 0.75), (int)(image.Height * 0.75), KnownResamplers.Lanczos3));
                    var imageRegular = webpImage.ToBase64String(format);

                    webpImage.Mutate(x => x.Resize(200, 200, KnownResamplers.Lanczos3));
                    var imageThumbnail = webpImage.ToBase64String(format);

                    var imageJsonDoc = new B64ImageStorage()
                    {
                        Regular = imageRegular,
                        Thumbnail = imageThumbnail,
                    };

                    var cleanPath = Path.Combine(_photoBaseDir, albumId.ToString(), photo.Name.Replace($".{initialExtension}", ".json"));
                    var json = JsonConvert.SerializeObject(imageJsonDoc, Formatting.Indented);
                    await File.WriteAllTextAsync(cleanPath, json);
                    var model = new Photo(cleanPath, DateTime.Now, albumId);
                    webpImage.Dispose();
                    File.Delete(webpPath);
                    photosToSave.Add(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "fuck");
                }
            };

            await _datbaseService.Photos.AddRangeAsync(photosToSave);
            await _datbaseService.SaveDbAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not upload photos");
            return Result.Error(ex.StackTrace);
        }
    }
}
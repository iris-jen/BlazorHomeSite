using Ardalis.Result;
using Ardalis.Specification;
using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using BlazorHomeSite.Services.Database;
using Newtonsoft.Json;
using SixLabors.ImageSharp.Formats;
using BlazorHomeSite.Data.Photos;

namespace BlazorHomeSite.Services.Photos;

public class UploadPhotoService : IUploadPhotoService
{
    private readonly IDatabaseService _datbaseService;
    private readonly ILogger<UploadPhotoService> _logger;
    private readonly string _photoBaseDir;

    public UploadPhotoService(IDatabaseService databaseService,
                               ILogger<UploadPhotoService> logger,
                               IOptions<AppAdminOptions> adminOptions)
    {
        _datbaseService = databaseService;
        _logger = logger;
        _photoBaseDir = adminOptions.Value.PhotoDirectory ??
                        Path.Combine(Environment.CurrentDirectory, "PhotoUploads");
    }

    public string SaveB64Json(B64ImageStorage b64, string albumId, string photoName, string initialExtension)
    {
        var cleanPath = Path.Combine(_photoBaseDir, albumId.ToString(), photoName.Replace($".{initialExtension}", ".json"));
        var json = JsonConvert.SerializeObject(b64, Formatting.Indented);
        File.WriteAllText(cleanPath, json);
        return cleanPath;
    }

    public async Task UploadPhotos(IReadOnlyList<IBrowserFile> inputPhotos, int albumId, int maxSizeBytes = 9999999)
    {
        try
        {
            var photos = new List<IBrowserFile>(inputPhotos);

            var albumIdNiceName = $"a_{albumId}";
            var targetDirectory = Path.Combine(_photoBaseDir, albumIdNiceName);
            Directory.CreateDirectory(targetDirectory);

            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 5
            };

            var photosToSave = new ConcurrentBag<Photo>();

            await Parallel.ForEachAsync(photos, parallelOptions, async (photo, _) =>
            {
                try
                {
                    var targetPath = Path.Combine(targetDirectory, $"{photo.Name}");
                    var initialExtension = targetPath.Split('.').Last();
                    var webpPath = targetPath.Replace($".{initialExtension}", ".webp");

                    await SaveUploadLocally(webpPath, photo, maxSizeBytes);
                    var b64 = GetB64Images(webpPath, photo.Name);
                    var b64JsonPath = SaveB64Json(b64, albumIdNiceName, photo.Name, initialExtension);

                    var model = new Photo(b64JsonPath, DateTime.Now, albumId);

                    photosToSave.Add(model);
                    _logger.LogInformation($"Upload Completed{photo.Name}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "error saving image as b64");
                }
            });

            await _datbaseService.Photos.AddRangeAsync(photosToSave);
            await _datbaseService.SaveDbAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not upload photos");
        }
    }

    private B64ImageStorage GetB64Images(string webpPath, string photoName)
    {
        _logger.LogInformation($"Photo: {photoName}, Saved as Webp ok: {webpPath}");

        var webpImage = Image.Load(webpPath);
        var format = Image.DetectFormat(webpPath);

        webpImage.Mutate(x => x.Resize((int)(webpImage.Width * 0.75), (int)(webpImage.Height * 0.75), KnownResamplers.Lanczos3));
        var imageRegular = webpImage.ToBase64String(format);

        webpImage.Mutate(x => x.Resize(200, 200, KnownResamplers.Lanczos3));
        var imageThumbnail = webpImage.ToBase64String(format);

        _logger.LogInformation($"Photo: {photoName} Mutations completed, Deleting source image..");
        File.Delete(webpPath);
        webpImage.Dispose();

        return new B64ImageStorage()
        {
            Regular = imageRegular,
            Thumbnail = imageThumbnail,
        };
    }

    private async Task SaveUploadLocally(string targetPath, IBrowserFile photo, int maxSizeBytes)
    {
        _logger.LogInformation($"Uploading photo {photo.Name}, Saving to {targetPath}");

        await using FileStream fs = new(targetPath, FileMode.Create);
        var image = await Image.LoadAsync(photo.OpenReadStream(maxSizeBytes));
        await image.SaveAsWebpAsync(fs);

        image.Dispose();
        fs.Dispose();

        _logger.LogInformation($"Photo {photo.Name} Copied to target path ok");
    }
}
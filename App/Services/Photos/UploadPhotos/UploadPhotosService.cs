using Ardalis.Specification;
using BlazorHomeSite.Data.Domain;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Concurrent;

namespace BlazorHomeSite.Services.Photos;

public class UploadPhotosService
{
    private readonly ILogger<UploadPhotosService> _logger;
    private readonly string _photoBaseDir;
    private readonly IRepositoryBase<Photo> _photoRepository;

    public UploadPhotosService(IRepositoryBase<Photo> photoRepository,
                                      ILogger<UploadPhotosService> logger,
                                      AppAdminOptions adminOptions)
    {
        _photoRepository = photoRepository;
        _logger = logger;
        _photoBaseDir = adminOptions.PhotoDirectory ??
                        Path.Combine(Environment.CurrentDirectory, "PhotoUploads");
    }

    public async Task UploadPhotos(IReadOnlyList<IBrowserFile> photos, int albumId, int maxSizeBytes = 9999999)
    {
        var targetDirectory = Path.Combine(_photoBaseDir, albumId.ToString());
        Directory.CreateDirectory(targetDirectory);

        ParallelOptions parallelOptions = new()
        {
            MaxDegreeOfParallelism = 10
        };

        var photosToSave = new ConcurrentBag<Photo>();

        await Parallel.ForEachAsync(photos, parallelOptions, async (photo, token) =>
        {
            var targetPath = Path.Combine(targetDirectory, $"{photo.Name}");
            await using FileStream fs = new(targetPath, FileMode.Create);
            await photo.OpenReadStream(maxSizeBytes, cancellationToken: token).CopyToAsync(fs, token);
            fs.Close();

            var image = Image.Load(targetPath);
            var webpPath = targetPath.Replace(".JPG", ".webp");
            image.SaveAsWebp(webpPath);
            var cleanPath = Path.Combine(_photoBaseDir, albumId.ToString(), photo.Name.Replace(".JPG", ".webp"));
            _logger.LogInformation("webp: " + webpPath);
            _logger.LogInformation("clean: " + cleanPath);
            var model = new Photo(cleanPath, cleanPath, DateTime.Now, albumId);

            photosToSave.Add(model);
        });

        await _photoRepository.AddRangeAsync(photosToSave);
        await _photoRepository.SaveChangesAsync();
    }
}
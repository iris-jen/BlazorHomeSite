using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Music;
using BlazorHomeSite.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Nextended.Blazor.Extensions;
using Nextended.Core.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System.Collections.Concurrent;
using System.IO;

namespace BlazorHomeSite.Services
{
    public class PhotoService : IPhotoRepository
    {
        private readonly IDbContextFactory<HomeSiteDbContext> _dbFactory;
        private readonly ILogger<PhotoService> _logger;
        private readonly string _photoDirectory;
        private const string PhotoDirectoryName = "PhotoUploads";

        public PhotoService(IDbContextFactory<HomeSiteDbContext> dbFactory,
                            ILogger<PhotoService> logger,
                            IWebHostEnvironment hostEnvironment)
        {
            _dbFactory = dbFactory;
            _logger = logger;
            _photoDirectory = Path.Combine(hostEnvironment.WebRootPath, PhotoDirectoryName);
            _logger.LogInformation($"Photo Service Constructed: {_photoDirectory}");
        }

        #region Photo Albums

        private string GetAlbumPath(int albumId) => Path.Combine(_photoDirectory, albumId.ToString());

        public async Task CreatePhotoAlbum(PhotoAlbum album)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            await context.PhotoAlbums.AddAsync(album);
            await context.SaveChangesAsync();

            Directory.CreateDirectory(GetAlbumPath(album.Id));
        }

        public async Task DeletePhotoAlbum(int photoAlbumID)
        {
            Directory.Delete(GetAlbumPath(photoAlbumID), true);

            var context = await _dbFactory.CreateDbContextAsync();
            await context.PhotoAlbums.Where(x => x.Id == photoAlbumID).ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }

        public async Task EditPhotoAlbum(PhotoAlbum album)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            context.PhotoAlbums.Update(album);

            await context.SaveChangesAsync();
        }

        public async Task<List<PhotoAlbum>> GetPhotoAlbums()
        {
            var context = await _dbFactory.CreateDbContextAsync();
            return await context.PhotoAlbums.ToListAsync();
        }

        #endregion Photo Albums

        #region Photos

        public async Task DeletePhoto(int photoID)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            await context.Photos.Where(x => x.Id == photoID).ExecuteDeleteAsync();
            await context.SaveChangesAsync();
        }

        public async Task EditPhoto(Photo photo)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            context.Photos.Update(photo);
            await context.SaveChangesAsync();
        }

        public async Task UploadPhotos(IReadOnlyList<IBrowserFile> photos, int albumId)
        {
            var context = await _dbFactory.CreateDbContextAsync();
            var targetDirectory = Path.Combine(_photoDirectory, albumId.ToString());
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
                await photo.OpenReadStream(99999999999, cancellationToken: token).CopyToAsync(fs, token);
                fs.Close();
               
                var image = Image.Load(targetPath);
                var webpPath = targetPath.Replace(".JPG", ".webp");
                image.SaveAsWebp(webpPath);
                var cleanPath = Path.Combine(PhotoDirectoryName, albumId.ToString(), photo.Name.Replace(".JPG", ".webp"));
                _logger.LogInformation("webp: " + webpPath);
                _logger.LogInformation("clean: " + cleanPath);
                var model = new Photo()
                {
                    AlbumId = albumId,
                    PhotoPath = cleanPath,
                    ThumbnailPath = cleanPath,
                };

                photosToSave.Add(model);
            });

            await context.AddRangeAsync(photosToSave);
            await context.SaveChangesAsync();
        }

        Task<List<Photo>> IPhotoRepository.GetPhotosByAlbum(int albumID)
        {
            throw new NotImplementedException();
        }

        #endregion Photos
    }
}
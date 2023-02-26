using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Music;
using BlazorHomeSite.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Nextended.Blazor.Extensions;
using SixLabors.ImageSharp;

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
                            IConfiguration config)
        {
            _dbFactory = dbFactory;
            _logger = logger;

            var configPhotoDir = config.GetSection("PhotoUploadFolder");
            var currentPhotoDir = Path.Combine(Directory.GetCurrentDirectory(), "photos");
            _photoDirectory = configPhotoDir.Value != null ?
                              configPhotoDir.Value.ToString() :
                              currentPhotoDir;
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

            foreach (var photo in photos)
            {
                var image = await Image.LoadWithFormatAsync(photo.OpenReadStream(9999999999));
                var targetPath = Path.Combine(targetDirectory, $"{photo.Name.Split(".")[0]}.webp");

                image.Image.SaveAsWebp(targetPath);

                var model = new Photo()
                {
                    AlbumId = albumId,
                    PhotoPath = targetPath,
                    ThumbnailPath = targetPath,
                };

                await context.Photos.AddAsync(model);
            }

            await context.SaveChangesAsync();
        }

        Task<List<Photo>> IPhotoRepository.GetPhotosByAlbum(int albumID)
        {
            throw new NotImplementedException();
        }

        #endregion Photos
    }
}
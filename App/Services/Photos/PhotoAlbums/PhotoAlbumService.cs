using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Services.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazorHomeSite.Services.Photos.PhotoAlbums
{
    public class PhotoAlbumService : IPhotoAlbumService
    {
        private readonly IDatabaseService _databaseService;
        private readonly string _photoBaseDir;

        public PhotoAlbumService(IDatabaseService databaseService, IOptions<AppAdminOptions> adminOptions)
        {
            _databaseService = databaseService;
            _photoBaseDir = adminOptions.Value.PhotoDirectory ??
                Path.Combine(Environment.CurrentDirectory, "PhotoUploads");
        }

        public async Task CreateNewPhotoAlbumAsync(string name, string description, UserLevel userLevel)
        {
            var album = new PhotoAlbum(name, description, userLevel);
            await _databaseService.PhotoAlbums.AddAsync(album);
            await _databaseService.SaveDbAsync();
        }

        public async Task DeletePhotoAlbumAsync(int id)
        {
            var directoryPath = Path.Combine(_photoBaseDir, $"a_{id}");
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            await _databaseService.Photos.Where(x => x.AlbumId == id).ExecuteDeleteAsync();
            await _databaseService.PhotoAlbums.Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public async Task EditPhotoAlbumAsync(int id, int? newOrder, string? newDescription,
                                                    UserLevel? newUserLevel, string? newName)
        {
            var album = await _databaseService.PhotoAlbums.FirstOrDefaultAsync(x => x.Id == id);
            if (album != null)
            {
                if (newOrder != null)
                    album.UpdateAlbumOrder(newOrder.Value);

                if (!string.IsNullOrEmpty(newDescription))
                    album.UpdateDescription(newDescription);

                if (newUserLevel != null)
                    album.UpdateUserLevel(album.UserLevel);

                if (!string.IsNullOrEmpty(newName))
                    album.UpdateName(newName);

                _databaseService.PhotoAlbums.Update(album);
                await _databaseService.SaveDbAsync();
            }
        }

        public async Task<List<PhotoAlbum>> GetAllAlbumsAsync()
        {
            return await _databaseService.PhotoAlbums.ToListAsync();
        }

        public async Task<PhotoAlbum?> GetPhotoAlbumByIdAsync(int id)
        {
            return await _databaseService.PhotoAlbums.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
using BlazorHomeSite.Data.Enums;
using BlazorHomeSite.Data.Photos;

namespace BlazorHomeSite.Services.Photos.PhotoAlbums
{
    public interface IPhotoAlbumService
    {
        Task CreateNewPhotoAlbumAsync(string name, string description, UserLevel userLevel);

        Task DeletePhotoAlbumAsync(int id);

        Task EditPhotoAlbumAsync(int id, int? newOrder, string? newDescription,
                                                    UserLevel? newUserLevel, string? newName);

        Task<List<PhotoAlbum>> GetAllAlbumsAsync();

        Task<PhotoAlbum?> GetPhotoAlbumByIdAsync(int id);
    }
}
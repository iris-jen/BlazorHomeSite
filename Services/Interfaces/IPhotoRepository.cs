using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorHomeSite.Services.Interfaces
{
    public interface IPhotoRepository
    {
        public Task<List<PhotoAlbum>> GetPhotoAlbums();

        public Task<List<Photo>> GetPhotosByAlbum(int albumId);

        public Task UploadPhotos(IReadOnlyList<IBrowserFile> photos, int albumId);

        public Task DeletePhoto(int photoId);

        public Task EditPhoto(Photo photo);

        public Task CreatePhotoAlbum(PhotoAlbum album);

        public Task EditPhotoAlbum(PhotoAlbum album);

        public Task DeletePhotoAlbum(int photoAlbumId);
    }
}
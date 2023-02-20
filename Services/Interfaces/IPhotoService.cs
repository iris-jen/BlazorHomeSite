using BlazorHomeSite.Data;

namespace BlazorHomeSite.Services.Interfaces
{
    public interface IPhotoService
    {
        public List<PhotoAlbum> GetPhotoAlbums();

        public List<Photo> GetPhotosByAlbum(int albumID);

        public Task UploadPhoto(Photo photo);

        public Task DeletePhoto(int photoID);

        public Task EditPhoto(Photo photo);

        public Task CreatePhotoAlbum(PhotoAlbum album);

        public Task EditPhotoAlbum(PhotoAlbum album);

        public Task DeletePhotoAlbum(int photoAlbumID);
    }
}